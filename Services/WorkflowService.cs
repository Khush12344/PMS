using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;

namespace PMS.Web.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly AppDbContext _db;
        private readonly IFileUploadService _fileUpload;
        private readonly ILogger<WorkflowService> _logger;

        public WorkflowService(AppDbContext db, IFileUploadService fileUpload, ILogger<WorkflowService> logger)
        {
            _db = db;
            _fileUpload = fileUpload;
            _logger = logger;
        }

        // ── Manager ───────────────────────────────────────────

        // Manager assigns petition to an OA
        public async Task AssignToOAAsync(int petitionId, int managerId, int oaUserId, string? remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status          = PetitionStatus.AssignedToOA;
            petition.AssignedManagerId = managerId;
            petition.AssignedOAId    = oaUserId;
            petition.AssignedToOAAt  = DateTime.UtcNow;
            petition.ManagerRemarks  = remarks;

            AddLog(petition, managerId, "AssignedToOA", "Under Review", remarks, from, PetitionStatus.AssignedToOA);
            await _db.SaveChangesAsync();
        }

        // Manager assigns OA for summarizing (after APCCF sends back)
        public async Task AssignForSummaryAsync(int petitionId, int managerId, int oaUserId, string? remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status        = PetitionStatus.PendingOASummary;
            petition.SummaryByOAId = oaUserId;
            petition.ManagerRemarks = remarks;

            AddLog(petition, managerId, "AssignedForSummary", "Under Review", remarks, from, PetitionStatus.PendingOASummary);
            await _db.SaveChangesAsync();
        }

        // ── Office Assistant ──────────────────────────────────

        public async Task ScrutinizeAsync(int petitionId, int oaUserId)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status              = PetitionStatus.UnderScrutiny;
            petition.ScrutinizedByUserId = oaUserId;
            petition.ScrutinizedAt       = DateTime.UtcNow;

            AddLog(petition, oaUserId, "UnderScrutiny", "Under Review", null, from, PetitionStatus.UnderScrutiny);
            await _db.SaveChangesAsync();
        }

        public async Task OADropAsync(int petitionId, int oaUserId, string dropReason)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status              = PetitionStatus.OADropped;
            petition.OADropReason        = dropReason;
            petition.OADroppedAt         = DateTime.UtcNow;
            petition.ScrutinizedByUserId = oaUserId;
            petition.ScrutinizedAt       = DateTime.UtcNow;

            AddLog(petition, oaUserId, "OADropped", "Under Review", dropReason, from, PetitionStatus.OADropped);
            await _db.SaveChangesAsync();
        }

        public async Task RecommendForActionAsync(int petitionId, int oaUserId, OfficeType officeType, int recommendedOfficerId, string? remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status               = PetitionStatus.RecommendedForAction;
            petition.OARecommendation     = OARecommendation.ForAction;
            petition.RecommendedOfficeType = officeType;
            petition.RecommendedOfficerId  = recommendedOfficerId;
            petition.OARemarks            = remarks;

            AddLog(petition, oaUserId, "RecommendedForAction", "Forwarded for Review", remarks, from, PetitionStatus.RecommendedForAction);
            await _db.SaveChangesAsync();
        }

        public async Task RecommendForDropAsync(int petitionId, int oaUserId, string reason, string remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status           = PetitionStatus.RecommendedForDrop;
            petition.OARecommendation = OARecommendation.ForDrop;
            petition.OARemarks        = $"Reason: {reason}. Remarks: {remarks}";

            AddLog(petition, oaUserId, "RecommendedForDrop", "Forwarded for Review", remarks, from, PetitionStatus.RecommendedForDrop);
            await _db.SaveChangesAsync();
        }

        // OA submits summary report (after APCCF sends to Manager → Manager assigns OA)
        public async Task SubmitSummaryAsync(int petitionId, int oaUserId, string summaryText, List<IFormFile> documents)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            var report = new ActionReport
            {
                PetitionId  = petitionId,
                OfficerId   = oaUserId,
                ReportType  = ReportType.Summary,
                ReportText  = summaryText,
                SubmittedAt = DateTime.UtcNow
            };
            _db.ActionReports.Add(report);
            await _db.SaveChangesAsync();

            if (documents.Any())
                await _fileUpload.SaveReportAttachmentsAsync(report.ReportId, documents, new List<IFormFile>());

            petition.Status              = PetitionStatus.SummarySubmitted;
            petition.SummarySubmittedAt  = DateTime.UtcNow;
            petition.SummaryRemarks      = summaryText;

            AddLog(petition, oaUserId, "SummarySubmitted", "Summary Submitted", null, from, PetitionStatus.SummarySubmitted);
            await _db.SaveChangesAsync();
        }

        // ── APCCF ─────────────────────────────────────────────

        public async Task AssignPetitionAsync(int petitionId, int apccfUserId, OfficeType officeType, int officerId, string? remarks, string? documentPath = null)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status             = PetitionStatus.Assigned;
            petition.AssignedOfficerId  = officerId;
            petition.AssignedOfficeType = officeType;
            petition.AssignedAt         = DateTime.UtcNow;
            petition.APCCFRemarks       = remarks;
            if (!string.IsNullOrEmpty(documentPath))
                petition.APCCFDocumentPath = documentPath;

            AddLog(petition, apccfUserId, "Assigned", "Assigned for Action", remarks, from, PetitionStatus.Assigned);
            await _db.SaveChangesAsync();
        }

        public async Task DropPetitionAsync(int petitionId, int apccfUserId, string remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status       = PetitionStatus.Dropped;
            petition.APCCFRemarks = remarks;

            AddLog(petition, apccfUserId, "Dropped", "Petition Closed", remarks, from, PetitionStatus.Dropped);
            await _db.SaveChangesAsync();
        }

        public async Task OverrideAndAssignAsync(int petitionId, int apccfUserId, OfficeType officeType, int officerId, string? remarks)
            => await AssignPetitionAsync(petitionId, apccfUserId, officeType, officerId, remarks);

        // APCCF sends petition back to Manager after FO report
        public async Task SendToManagerAsync(int petitionId, int apccfUserId, string? remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            petition.Status       = PetitionStatus.SentToManager;
            petition.APCCFRemarks = remarks;

            AddLog(petition, apccfUserId, "SentToManager", "Sent for Review", remarks, from, PetitionStatus.SentToManager);
            await _db.SaveChangesAsync();
        }

        // ── Field Officer ─────────────────────────────────────

        public async Task SubmitInterimReportAsync(int petitionId, int officerId, string reportText, List<IFormFile> documents, List<IFormFile> photos)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            var report = new ActionReport
            {
                PetitionId  = petitionId,
                OfficerId   = officerId,
                ReportType  = ReportType.Interim,
                ReportText  = reportText,
                SubmittedAt = DateTime.UtcNow
            };
            _db.ActionReports.Add(report);
            await _db.SaveChangesAsync();

            await _fileUpload.SaveReportAttachmentsAsync(report.ReportId, documents, photos);

            petition.Status = PetitionStatus.InProgress;
            AddLog(petition, officerId, "InterimReportSubmitted", "Action In Progress", null, from, PetitionStatus.InProgress);
            await _db.SaveChangesAsync();
        }

        public async Task SubmitFinalReportAsync(int petitionId, int officerId, string reportText, List<IFormFile> documents, List<IFormFile> photos)
        {
            var petition = await GetPetitionAsync(petitionId);
            var from = petition.Status;

            var report = new ActionReport
            {
                PetitionId  = petitionId,
                OfficerId   = officerId,
                ReportType  = ReportType.Final,
                ReportText  = reportText,
                SubmittedAt = DateTime.UtcNow
            };
            _db.ActionReports.Add(report);
            await _db.SaveChangesAsync();

            await _fileUpload.SaveReportAttachmentsAsync(report.ReportId, documents, photos);

            petition.Status = PetitionStatus.SubmittedForClosure;
            AddLog(petition, officerId, "FinalReportSubmitted", "Submitted for Review", null, from, PetitionStatus.SubmittedForClosure);
            await _db.SaveChangesAsync();
        }

        // ── APCCF Final Decision ──────────────────────────────

        public async Task AcceptFinalReportAsync(int petitionId, int apccfUserId, int reportId, string? remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var report   = await _db.ActionReports.FindAsync(reportId)
                           ?? throw new InvalidOperationException("Report not found");
            var from = petition.Status;

            report.IsApproved           = true;
            report.APCCFDecisionRemarks = remarks;
            report.DecisionAt           = DateTime.UtcNow;

            petition.Status = PetitionStatus.Closed;
            AddLog(petition, apccfUserId, "Closed", "Petition Closed", remarks, from, PetitionStatus.Closed);
            await _db.SaveChangesAsync();
        }


        public async Task RejectAndResendAsync(int petitionId, int apccfUserId, int reportId, string remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var report   = await _db.ActionReports.FindAsync(reportId)
                           ?? throw new InvalidOperationException("Report not found");
            var from = petition.Status;

            report.IsApproved           = false;
            report.APCCFDecisionRemarks = remarks;
            report.DecisionAt           = DateTime.UtcNow;

            petition.Status = PetitionStatus.Reopened;
            AddLog(petition, apccfUserId, "ReopenedSameOfficer", "Sent Back for Revision", remarks, from, PetitionStatus.Reopened);
            await _db.SaveChangesAsync();
        }

        public async Task RejectAndReassignAsync(int petitionId, int apccfUserId, int reportId, OfficeType officeType, int newOfficerId, string remarks)
        {
            var petition = await GetPetitionAsync(petitionId);
            var report   = await _db.ActionReports.FindAsync(reportId)
                           ?? throw new InvalidOperationException("Report not found");
            var from = petition.Status;

            report.IsApproved           = false;
            report.APCCFDecisionRemarks = remarks;
            report.DecisionAt           = DateTime.UtcNow;

            petition.Status             = PetitionStatus.Assigned;
            petition.AssignedOfficerId  = newOfficerId;
            petition.AssignedOfficeType = officeType;
            petition.AssignedAt         = DateTime.UtcNow;
            petition.APCCFRemarks       = remarks;

            AddLog(petition, apccfUserId, "ReassignedNewOfficer", "Reassigned to New Officer", remarks, from, PetitionStatus.Assigned);
            await _db.SaveChangesAsync();
        }

        // ── Helpers ───────────────────────────────────────────

        private async Task<Petition> GetPetitionAsync(int petitionId)
            => await _db.Petitions.FindAsync(petitionId)
               ?? throw new InvalidOperationException($"Petition {petitionId} not found");

        private void AddLog(Petition petition, int? actorUserId, string action, string publicLabel,
            string? internalRemarks, PetitionStatus from, PetitionStatus to)
        {
            _db.PetitionWorkflowLogs.Add(new PetitionWorkflowLog
            {
                PetitionId      = petition.PetitionId,
                ActorUserId     = actorUserId,
                Action          = action,
                PublicLabel     = publicLabel,
                InternalRemarks = internalRemarks,
                FromStatus      = from,
                ToStatus        = to,
                ActionAt        = DateTime.UtcNow
            });
        }
    }
}
