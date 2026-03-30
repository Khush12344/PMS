using PMS.Web.Models.Enums;

namespace PMS.Web.Models.Entities
{
    public class PetitionAttachment
    {
        public int AttachmentId { get; set; }
        public int PetitionId { get; set; }
        public AttachmentType FileType { get; set; }   // Document (PDF) or Photo (JPG)
        public string StoredFileName { get; set; } = string.Empty;   // GUID-based stored name
        public string OriginalFileName { get; set; } = string.Empty; // Original upload name
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Petition Petition { get; set; } = null!;
    }
}
