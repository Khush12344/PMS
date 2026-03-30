using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using PMS.Web.Services;

namespace PMS.Web.Pages.Manager
{
    [Authorize(Roles = "Manager")]
    public class PetitionDetailModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IWorkflowService _workflow;

        public PetitionDetailModel(AppDbContext db, IWorkflowService workflow)
        {
            _db = db;
            _workflow = workflow;
        }

        public Petition? Petition { get; set; }
        public List<User> OAUsers { get; set; } = new();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        [BindProperty] public int PetitionId { get; set; }
        [BindProperty] public int? SelectedOAId { get; set; }
        [BindProperty] public string? Remarks { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadAsync(id);
            if (Petition == null) return RedirectToPage("/Manager/Dashboard");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var managerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            try
            {
                switch (action)
                {
                    // Assign OA for scrutiny (new petition)
                    case "assignoa":
                        if (SelectedOAId == null)
                        { ErrorMessage = "Please select an OA."; break; }
                        await _workflow.AssignToOAAsync(PetitionId, managerId, SelectedOAId.Value, Remarks);
                        SuccessMessage = "✓ Petition assigned to OA for scrutiny.";
                        break;

                    // Assign OA for summary (sent back from APCCF)
                    case "assignsummary":
                        if (SelectedOAId == null)
                        { ErrorMessage = "Please select an OA for summarizing."; break; }
                        await _workflow.AssignForSummaryAsync(PetitionId, managerId, SelectedOAId.Value, Remarks);
                        SuccessMessage = "✓ Petition assigned to OA for summary.";
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }

            await LoadAsync(PetitionId);
            return Page();
        }

        private async Task LoadAsync(int petitionId)
        {
            Petition = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.LocationTaluk)
                .Include(p => p.ComplainantDistrict)
                .Include(p => p.ComplainantTaluk)
                .Include(p => p.AssignedOA)
                .Include(p => p.AssignedOfficer)
                .Include(p => p.Attachments)
                .Include(p => p.WorkflowLogs.OrderBy(l => l.ActionAt))
                    .ThenInclude(l => l.ActorUser)
                .Include(p => p.ActionReports.OrderByDescending(r => r.SubmittedAt))
                .FirstOrDefaultAsync(p => p.PetitionId == petitionId);

            // Load OA users
            OAUsers = await _db.Users
                .Where(u => u.IsActive && u.Role == UserRole.OfficeAssistant)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }
    }
}
