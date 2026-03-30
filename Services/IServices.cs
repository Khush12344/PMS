using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using PMS.Web.Models.ViewModels;

namespace PMS.Web.Services
{
    // ── OTP Service ───────────────────────────────────────────
    public interface IOtpService
    {
        Task<string> GenerateAndSaveOtpAsync(string mobileNumber);
        Task<bool> ValidateOtpAsync(string mobileNumber, string otp);
    }

    // ── Petition Service ──────────────────────────────────────
    public interface IPetitionService
    {
        Task<Petition> SubmitPetitionAsync(PetitionSubmitViewModel model);
        Task<Petition?> GetByApplicationIdAsync(string applicationId);
        Task<Petition?> GetByIdAsync(int petitionId);
        Task<List<Petition>> GetByStatusAsync(PetitionStatus status);
        Task<List<Petition>> GetAllAsync();
        Task<List<Petition>> GetAssignedToOfficerAsync(int officerId);
        Task<string> GenerateNextApplicationIdAsync();
    }

    // ── Workflow Service ──────────────────────────────────────
    public interface IWorkflowService
    {
        // Manager actions
        Task AssignToOAAsync(int petitionId, int managerId, int oaUserId, string? remarks);
        Task AssignForSummaryAsync(int petitionId, int managerId, int oaUserId, string? remarks);

        // Office Assistant actions
        Task ScrutinizeAsync(int petitionId, int oaUserId);
        Task OADropAsync(int petitionId, int oaUserId, string dropReason);
        Task RecommendForActionAsync(int petitionId, int oaUserId, OfficeType officeType, int recommendedOfficerId, string? remarks);
        Task RecommendForDropAsync(int petitionId, int oaUserId, string reason, string remarks);
        Task SubmitSummaryAsync(int petitionId, int oaUserId, string summaryText, List<IFormFile> documents);

        // APCCF actions
        Task AssignPetitionAsync(int petitionId, int apccfUserId, OfficeType officeType, int officerId, string? remarks, string? documentPath = null);
        Task DropPetitionAsync(int petitionId, int apccfUserId, string remarks);
        Task OverrideAndAssignAsync(int petitionId, int apccfUserId, OfficeType officeType, int officerId, string? remarks);
        Task SendToManagerAsync(int petitionId, int apccfUserId, string? remarks);

        // Field Officer actions
        Task SubmitInterimReportAsync(int petitionId, int officerId, string reportText, List<IFormFile> documents, List<IFormFile> photos);
        Task SubmitFinalReportAsync(int petitionId, int officerId, string reportText, List<IFormFile> documents, List<IFormFile> photos);

        // APCCF Final decision
        Task AcceptFinalReportAsync(int petitionId, int apccfUserId, int reportId, string? remarks);
        Task RejectAndResendAsync(int petitionId, int apccfUserId, int reportId, string remarks);
        Task RejectAndReassignAsync(int petitionId, int apccfUserId, int reportId, OfficeType officeType, int newOfficerId, string remarks);
    }

    // ── File Upload Service ───────────────────────────────────
    public interface IFileUploadService
    {
        Task<List<string>> SavePetitionAttachmentsAsync(int petitionId, List<IFormFile> documents, List<IFormFile> photos);
        Task<List<string>> SaveReportAttachmentsAsync(int reportId, List<IFormFile> documents, List<IFormFile> photos);
        Task<string> SaveAPCCFDocumentAsync(int petitionId, IFormFile document);
    }

    // ── Notification Service ──────────────────────────────────
    public interface INotificationService
    {
        Task SendSmsAsync(string mobileNumber, string message);
        Task SendPetitionAcknowledgementAsync(string mobileNumber, string applicationId);
        Task SendOtpAsync(string mobileNumber, string otp);
    }
}
