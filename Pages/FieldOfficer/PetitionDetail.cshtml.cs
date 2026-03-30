using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using PMS.Web.Services;

namespace PMS.Web.Pages.FieldOfficer
{
    [Authorize(Roles = "CircleOfficer,DivisionOfficer,FMSOfficer")]
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
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        [BindProperty] public int PetitionId { get; set; }
        [BindProperty] public string ReportText { get; set; } = string.Empty;
        [BindProperty] public IFormFile? Document1 { get; set; }
        [BindProperty] public IFormFile? Document2 { get; set; }
        [BindProperty] public IFormFile? Photo1 { get; set; }
        [BindProperty] public IFormFile? Photo2 { get; set; }
        [BindProperty] public IFormFile? Photo3 { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadAsync(id);
            if (Petition == null) return RedirectToPage("/FieldOfficer/Dashboard");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var officerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            if (string.IsNullOrWhiteSpace(ReportText))
            {
                ErrorMessage = "Please enter report details.";
                await LoadAsync(PetitionId);
                return Page();
            }

            var docs   = new List<IFormFile?> { Document1, Document2 }.Where(f => f != null).Cast<IFormFile>().ToList();
            var photos = new List<IFormFile?> { Photo1, Photo2, Photo3 }.Where(f => f != null).Cast<IFormFile>().ToList();

            try
            {
                switch (action)
                {
                    case "interim":
                        await _workflow.SubmitInterimReportAsync(PetitionId, officerId, ReportText, docs, photos);
                        SuccessMessage = "✓ Interim report submitted successfully.";
                        break;

                    case "final":
                        await _workflow.SubmitFinalReportAsync(PetitionId, officerId, ReportText, docs, photos);
                        SuccessMessage = "✓ Final report submitted. Awaiting APCCF approval.";
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
            var officerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            Petition = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.LocationTaluk)
                .Include(p => p.ComplainantDistrict)
                .Include(p => p.ComplainantTaluk)
                .Include(p => p.Attachments)
                .Include(p => p.WorkflowLogs.OrderBy(l => l.ActionAt))
                .Include(p => p.ActionReports.OrderByDescending(r => r.SubmittedAt))
                    .ThenInclude(r => r.Attachments)
                .FirstOrDefaultAsync(p =>
                    p.PetitionId == petitionId &&
                    p.AssignedOfficerId == officerId); // Security: only assigned officer
        }
    }
}
