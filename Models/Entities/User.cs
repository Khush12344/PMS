using PMS.Web.Models.Enums;

namespace PMS.Web.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserRole Role { get; set; }
        public OfficeType? OfficeType { get; set; }

        // ── Address fields (for public users) ────────────────
        public string? HouseNo { get; set; }
        public string? Street { get; set; }
        public string? Area { get; set; }
        public int? DistrictId { get; set; }
        public int? TalukId { get; set; }
        public string? Village { get; set; }
        public string? Pincode { get; set; }

        // ── Auth ──────────────────────────────────────────────
        public string? OtpHash { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ── Navigation ────────────────────────────────────────
        public District? District { get; set; }
        public Taluk? Taluk { get; set; }
        public ICollection<Petition> SubmittedPetitions { get; set; } = new List<Petition>();
        public ICollection<Petition> AssignedPetitions { get; set; } = new List<Petition>();
        public ICollection<PetitionWorkflowLog> WorkflowLogs { get; set; } = new List<PetitionWorkflowLog>();
        public ICollection<ActionReport> ActionReports { get; set; } = new List<ActionReport>();
    }
}
