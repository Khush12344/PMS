using PMS.Web.Models.Enums;

namespace PMS.Web.Models.Entities
{
    /// <summary>
    /// Records every stage transition of a petition.
    /// The public tracking view reads from this table - showing only
    /// stage name + timestamp (no internal remarks or documents).
    /// </summary>
    public class PetitionWorkflowLog
    {
        public int LogId { get; set; }
        public int PetitionId { get; set; }
        public int? ActorUserId { get; set; }              // Null for system-generated entries
        public string Action { get; set; } = string.Empty; // e.g. "Submitted", "ForwardedToAPCCF"
        public string? PublicLabel { get; set; }           // What public sees e.g. "Under Review"
        public string? InternalRemarks { get; set; }       // Internal only - never shown to public
        public PetitionStatus FromStatus { get; set; }
        public PetitionStatus ToStatus { get; set; }
        public DateTime ActionAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Petition Petition { get; set; } = null!;
        public User? ActorUser { get; set; }
    }
}
