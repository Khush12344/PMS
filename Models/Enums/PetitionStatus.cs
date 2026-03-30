namespace PMS.Web.Models.Enums
{
    public enum PetitionStatus
    {
        Submitted,                  // Public submitted — waiting for Manager
        AssignedToOA,               // Manager assigned to specific OA
        UnderScrutiny,              // OA is reviewing
        OADropped,                  // OA dropped directly — APCCF can override
        RecommendedForAction,       // OA recommended for action → APCCF
        RecommendedForDrop,         // OA recommended drop → APCCF
        Assigned,                   // APCCF assigned to Field Officer
        Dropped,                    // APCCF final drop
        InProgress,                 // Field Officer submitted interim report
        SubmittedForClosure,        // Field Officer submitted final report → APCCF
        SentToManager,              // APCCF sent to Manager for review
        PendingOASummary,           // Manager assigned OA to summarize
        SummarySubmitted,           // OA submitted summary → APCCF final
        Closed,                     // APCCF accepted and closed
        Reopened                    // APCCF rejected and sent back to FO
    }

    public enum UserRole
    {
        Public,
        Manager,                    // NEW — assigns OA, reviews FO reports
        OfficeAssistant,
        APCCF,
        CircleOfficer,
        DivisionOfficer,
        FMSOfficer
    }

    public enum AttachmentType
    {
        Document,   // PDF
        Photo       // JPG/JPEG
    }

    public enum ReportType
    {
        Interim,
        Final,
        Summary     // OA summary report
    }

    public enum OfficeType
    {
        Circle,
        Division,
        FMSDivision
    }

    public enum OARecommendation
    {
        ForAction,
        ForDrop
    }
}
