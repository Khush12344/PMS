using PMS.Web.Models.Enums;

namespace PMS.Web.Models.Entities
{
    public class Petition
    {
        public int? CreatedByUserId { get; set; }
        public string? CreatedByRole { get; set; }
        public int PetitionId { get; set; }
        public string PetitionApplicationId { get; set; } = string.Empty;

        // ── Complainant Info ──────────────────────────────────────
        public string? ComplainantName { get; set; }
        public string? HouseNo { get; set; }
        public string? Street { get; set; }
        public string? Area { get; set; }
        public int? ComplainantDistrictId { get; set; }
        public bool? ComplainantIsUrban { get; set; }
        public int? ComplainantTalukId { get; set; }
        public int? ComplainantHobliId { get; set; }
        public int? ComplainantVillageId { get; set; }
        public int? ComplainantTownId { get; set; }
        public string? ComplainantVillage { get; set; }
        public string? ComplainantPincode { get; set; }

        // ── Complaint Details ─────────────────────────────────────
        public int ComplaintCategoryId { get; set; }
        public string Description { get; set; } = string.Empty;

        // ── Incident Location ─────────────────────────────────────
        public int LocationDistrictId { get; set; }
        public bool? LocationIsUrban { get; set; }
        public int? LocationTalukId { get; set; }
        public int? LocationHobliId { get; set; }
        public int? LocationVillageId { get; set; }
        public int? LocationTownId { get; set; }
        public string? LocationVillage { get; set; }
        public string? LocationPincode { get; set; }

        // ── Contact ───────────────────────────────────────────────
        public string MobileNumber { get; set; } = string.Empty;

        // ── Status & Workflow ─────────────────────────────────────
        public PetitionStatus Status { get; set; } = PetitionStatus.Submitted;
        public bool IsOfflineEntry { get; set; } = false;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // ── Manager Assignment ────────────────────────────────────
        public int? AssignedManagerId { get; set; }       // Manager who handled this
        public int? AssignedOAId { get; set; }             // OA assigned by Manager
        public DateTime? AssignedToOAAt { get; set; }
        public string? ManagerRemarks { get; set; }

        // ── Office Assistant Scrutiny ─────────────────────────────
        public int? ScrutinizedByUserId { get; set; }
        public DateTime? ScrutinizedAt { get; set; }
        public OARecommendation? OARecommendation { get; set; }
        public OfficeType? RecommendedOfficeType { get; set; }
        public int? RecommendedOfficerId { get; set; }
        public string? OARemarks { get; set; }
        public string? OADropReason { get; set; }
        public DateTime? OADroppedAt { get; set; }

        // ── APCCF Assignment ──────────────────────────────────────
        public int? AssignedOfficerId { get; set; }
        public OfficeType? AssignedOfficeType { get; set; }
        public DateTime? AssignedAt { get; set; }
        public string? APCCFRemarks { get; set; }
        public string? APCCFDocumentPath { get; set; }     // Optional PDF APCCF uploads

        // ── Summary (OA after FO report) ──────────────────────────
        public int? SummaryByOAId { get; set; }            // OA who wrote summary
        public DateTime? SummarySubmittedAt { get; set; }
        public string? SummaryRemarks { get; set; }

        // ── Navigation Properties ─────────────────────────────────
        public District? ComplainantDistrict { get; set; }
        public Taluk? ComplainantTaluk { get; set; }
        public Hobli? ComplainantHobli { get; set; }
        public Village? ComplainantVillageNav { get; set; }
        public Town? ComplainantTown { get; set; }

        public District? LocationDistrict { get; set; }
        public Taluk? LocationTaluk { get; set; }
        public Hobli? LocationHobli { get; set; }
        public Village? LocationVillageNav { get; set; }
        public Town? LocationTown { get; set; }

        public ComplaintCategory? ComplaintCategory { get; set; }

        public User? AssignedManager { get; set; }
        public User? AssignedOA { get; set; }
        public User? AssignedOfficer { get; set; }
        public User? ScrutinizedBy { get; set; }
        public User? RecommendedOfficer { get; set; }
        public User? SummaryByOA { get; set; }

        public ICollection<PetitionAttachment> Attachments { get; set; } = new List<PetitionAttachment>();
        public ICollection<PetitionWorkflowLog> WorkflowLogs { get; set; } = new List<PetitionWorkflowLog>();
        public ICollection<ActionReport> ActionReports { get; set; } = new List<ActionReport>();



    }
}
