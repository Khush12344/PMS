using PMS.Web.Models.Enums;

namespace PMS.Web.Models.Entities
{
    public class ActionReport
    {
        public int ReportId { get; set; }
        public int PetitionId { get; set; }
        public int OfficerId { get; set; }
        public ReportType ReportType { get; set; }         // Interim or Final
        public string ReportText { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Final report APCCF decision
        public bool? IsApproved { get; set; }              // null=pending, true=accepted, false=rejected
        public string? APCCFDecisionRemarks { get; set; }
        public DateTime? DecisionAt { get; set; }

        // Navigation
        public Petition Petition { get; set; } = null!;
        public User Officer { get; set; } = null!;
        public ICollection<ActionReportAttachment> Attachments { get; set; } = new List<ActionReportAttachment>();
    }

    public class ActionReportAttachment
    {
        public int AttachmentId { get; set; }
        public int ReportId { get; set; }
        public AttachmentType FileType { get; set; }       // Document (PDF) or Photo (JPG)
        public string StoredFileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ActionReport Report { get; set; } = null!;
    }
}
