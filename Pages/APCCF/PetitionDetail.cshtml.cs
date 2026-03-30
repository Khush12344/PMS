using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using PMS.Web.Services;

namespace PMS.Web.Pages.APCCF
{
    [Authorize(Roles = "APCCF")]
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
        [BindProperty] public int? AssignedOfficerId { get; set; }
        [BindProperty] public string? DropReason { get; set; }
        [BindProperty] public int? ReportId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadAsync(id);
            if (Petition == null) return RedirectToPage("/APCCF/Dashboard");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var apccfUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            try
            {
                switch (action)
                {
                    // ── Assign to field officer ───────────────────
                    case "assign":
                        if (string.IsNullOrWhiteSpace(OfficeType) || AssignedOfficerId == null)
                        { ErrorMessage = "Please select Office Type and Officer."; break; }
                        if (!Enum.TryParse<OfficeType>(OfficeType, out var ot))
                        { ErrorMessage = "Invalid office type."; break; }
                        await _workflow.AssignPetitionAsync(PetitionId, apccfUserId, ot, AssignedOfficerId.Value, Remarks);
                        SuccessMessage = "✓ Petition assigned to field officer successfully.";
                        break;

                    // ── Override OA drop and assign ───────────────
                    case "override":
                        if (string.IsNullOrWhiteSpace(OfficeType) || AssignedOfficerId == null)
                        { ErrorMessage = "Please select Office Type and Officer to assign."; break; }
                        if (!Enum.TryParse<OfficeType>(OfficeType, out var ot2))
                        { ErrorMessage = "Invalid office type."; break; }
                        await _workflow.OverrideAndAssignAsync(PetitionId, apccfUserId, ot2, AssignedOfficerId.Value, Remarks);
                        SuccessMessage = "✓ OA decision overridden. Petition assigned to field officer.";
                        break;

                    // ── Drop petition ─────────────────────────────
                    case "drop":
                        if (string.IsNullOrWhiteSpace(DropReason))
                        { ErrorMessage = "Please provide a reason for dropping."; break; }
                        await _workflow.DropPetitionAsync(PetitionId, apccfUserId, DropReason);
                        SuccessMessage = "✓ Petition dropped.";
                        break;

                    // ── Accept final report → Close ───────────────
                    case "accept":
                        if (ReportId == null)
                        { ErrorMessage = "Report not found."; break; }
                        await _workflow.AcceptFinalReportAsync(PetitionId, apccfUserId, ReportId.Value, Remarks);
                        SuccessMessage = "✓ Final report accepted. Petition closed.";
                        break;

                    // ── Reject and resend to same officer ─────────
                    case "resend":
                        if (ReportId == null || string.IsNullOrWhiteSpace(Remarks))
                        { ErrorMessage = "Please provide remarks for rejection."; break; }
                        await _workflow.RejectAndResendAsync(PetitionId, apccfUserId, ReportId.Value, Remarks);
                        SuccessMessage = "✓ Report rejected. Sent back to same officer.";
                        break;

                    // ── Reject and reassign to new officer ────────
                    case "reassign":
                        if (ReportId == null || string.IsNullOrWhiteSpace(OfficeType) || AssignedOfficerId == null)
                        { ErrorMessage = "Please select a new officer to reassign."; break; }
                        if (!Enum.TryParse<OfficeType>(OfficeType, out var ot3))
                        { ErrorMessage = "Invalid office type."; break; }
                        await _workflow.RejectAndReassignAsync(PetitionId, apccfUserId, ReportId.Value, ot3, AssignedOfficerId.Value, Remarks ?? "");
                        SuccessMessage = "✓ Report rejected. Petition reassigned to new officer.";
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
                .Include(p => p.ScrutinizedBy)
                .Include(p => p.RecommendedOfficer)
                .Include(p => p.AssignedOfficer)
                .Include(p => p.WorkflowLogs.OrderBy(l => l.ActionAt))
                    .ThenInclude(l => l.ActorUser)
                .Include(p => p.ActionReports.OrderByDescending(r => r.SubmittedAt))
                    .ThenInclude(r => r.Attachments)
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
