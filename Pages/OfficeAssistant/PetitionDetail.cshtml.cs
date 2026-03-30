using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using PMS.Web.Services;

namespace PMS.Web.Pages.OfficeAssistant
{
    [Authorize(Roles = "OfficeAssistant")]
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
        public List<User> FieldOfficers { get; set; } = new();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        [BindProperty] public int PetitionId { get; set; }
        [BindProperty] public string? Remarks { get; set; }
        [BindProperty] public string? OfficeType { get; set; }
        [BindProperty] public int? RecommendedOfficerId { get; set; }
        [BindProperty] public string? DropReason { get; set; }
        [BindProperty] public string? OADropReason { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadAsync(id);
            if (Petition == null) return RedirectToPage("/OfficeAssistant/Dashboard");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var oaUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            try
            {
                switch (action)
                {
                    case "scrutinize":
                        await _workflow.ScrutinizeAsync(PetitionId, oaUserId);
                        SuccessMessage = "Petition marked as Under Scrutiny. Review the documents and take action.";
                        break;

                    case "foraction":
                        if (string.IsNullOrWhiteSpace(OfficeType) || RecommendedOfficerId == null)
                        { ErrorMessage = "Please select Office Type and Recommended Officer."; break; }
                        if (!Enum.TryParse<OfficeType>(OfficeType, out var ot))
                        { ErrorMessage = "Invalid office type."; break; }
                        await _workflow.RecommendForActionAsync(PetitionId, oaUserId, ot, RecommendedOfficerId.Value, Remarks);
                        SuccessMessage = "✓ Petition forwarded to APCCF with recommendation FOR ACTION.";
                        break;

                    case "fordrop":
                        if (string.IsNullOrWhiteSpace(DropReason))
                        { ErrorMessage = "Please provide a reason for recommending drop."; break; }
                        await _workflow.RecommendForDropAsync(PetitionId, oaUserId, DropReason, Remarks ?? "");
                        SuccessMessage = "✓ Petition forwarded to APCCF with recommendation FOR DROP.";
                        break;

                    case "oadrop":
                        if (string.IsNullOrWhiteSpace(OADropReason))
                        { ErrorMessage = "Please provide a reason for dropping this petition."; break; }
                        await _workflow.OADropAsync(PetitionId, oaUserId, OADropReason);
                        SuccessMessage = "✓ Petition dropped. APCCF has been notified and can review the original.";
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
                .Include(p => p.Attachments)
                .Include(p => p.WorkflowLogs.OrderBy(l => l.ActionAt))
                .FirstOrDefaultAsync(p => p.PetitionId == petitionId);

            FieldOfficers = await _db.Users
                .Where(u => u.IsActive && (
                    u.Role == UserRole.CircleOfficer ||
                    u.Role == UserRole.DivisionOfficer ||
                    u.Role == UserRole.FMSOfficer))
                .OrderBy(u => u.Name)
                .ToListAsync();
        }
    }
}
