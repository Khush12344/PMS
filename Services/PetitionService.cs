using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Helpers;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using PMS.Web.Models.ViewModels;

namespace PMS.Web.Services
{
    public class PetitionService : IPetitionService
    {
        private readonly AppDbContext _db;
        private readonly IFileUploadService _fileUpload;
        private readonly INotificationService _notification;
        private readonly ILogger<PetitionService> _logger;

        public PetitionService(AppDbContext db, IFileUploadService fileUpload,
            INotificationService notification, ILogger<PetitionService> logger)
        {
            _db = db;
            _fileUpload = fileUpload;
            _notification = notification;
            _logger = logger;
        }

        public async Task<Petition> SubmitPetitionAsync(PetitionSubmitViewModel model)
        {
            string appId = await GenerateNextApplicationIdAsync();

            var petition = new Petition
            {
                PetitionApplicationId = appId,
                ComplainantName       = model.ComplainantName,
                HouseNo               = model.HouseNo,
                Street                = model.Street,
                Area                  = model.Area,
                ComplainantDistrictId = model.ComplainantDistrictId,
                ComplainantTalukId    = model.ComplainantTalukId,
                ComplainantVillage    = model.ComplainantVillage,
                ComplainantPincode    = model.ComplainantPincode,
                ComplaintCategoryId   = model.ComplaintCategoryId,
                Description           = model.Description,
                LocationDistrictId    = model.LocationDistrictId,
                LocationTalukId       = model.LocationTalukId,
                LocationVillage       = model.LocationVillage,
                LocationPincode       = model.LocationPincode,
                MobileNumber          = model.MobileNumber,
                Status                = PetitionStatus.Submitted,
                SubmittedAt           = DateTime.UtcNow
            };

            _db.Petitions.Add(petition);
            await _db.SaveChangesAsync();

            // Save attachments
            if (model.Documents?.Any() == true || model.Photos?.Any() == true)
                await _fileUpload.SavePetitionAttachmentsAsync(petition.PetitionId,
                    model.Documents ?? new(), model.Photos ?? new());

            // Log workflow entry (public label shown on tracking page)
            _db.PetitionWorkflowLogs.Add(new PetitionWorkflowLog
            {
                PetitionId      = petition.PetitionId,
                Action          = "Submitted",
                PublicLabel     = "Petition Submitted",
                FromStatus      = PetitionStatus.Submitted,
                ToStatus        = PetitionStatus.Submitted,
                ActionAt        = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();

            // Send SMS acknowledgement
            await _notification.SendPetitionAcknowledgementAsync(model.MobileNumber, appId);

            _logger.LogInformation("Petition {AppId} submitted from {Mobile}", appId, model.MobileNumber);
            return petition;
        }

        public async Task<string> GenerateNextApplicationIdAsync()
        {
            int year = DateTime.UtcNow.Year;
            string prefix = $"PMS-{year}-";

            // Get highest sequence number for this year
            var lastId = await _db.Petitions
                .Where(p => p.PetitionApplicationId.StartsWith(prefix))
                .OrderByDescending(p => p.PetitionApplicationId)
                .Select(p => p.PetitionApplicationId)
                .FirstOrDefaultAsync();

            int nextSeq = 1;
            if (lastId != null)
            {
                var parts = lastId.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int last))
                    nextSeq = last + 1;
            }

            return PetitionIdGenerator.Generate(nextSeq, year);
        }

        public async Task<Petition?> GetByApplicationIdAsync(string applicationId)
            => await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.LocationTaluk)
                .Include(p => p.AssignedOfficer)
                .Include(p => p.WorkflowLogs.OrderBy(l => l.ActionAt))
                .FirstOrDefaultAsync(p => p.PetitionApplicationId == applicationId);

        public async Task<Petition?> GetByIdAsync(int petitionId)
            => await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.LocationTaluk)
                .Include(p => p.ComplainantDistrict)
                .Include(p => p.ComplainantTaluk)
                .Include(p => p.AssignedOfficer)
                .Include(p => p.ScrutinizedBy)
                .Include(p => p.RecommendedOfficer)
                .Include(p => p.Attachments)
                .Include(p => p.WorkflowLogs.OrderBy(l => l.ActionAt))
                    .ThenInclude(l => l.ActorUser)
                .Include(p => p.ActionReports.OrderBy(r => r.SubmittedAt))
                    .ThenInclude(r => r.Attachments)
                .FirstOrDefaultAsync(p => p.PetitionId == petitionId);

        public async Task<List<Petition>> GetByStatusAsync(PetitionStatus status)
            => await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.SubmittedAt)
                .ToListAsync();

        public async Task<List<Petition>> GetAllAsync()
            => await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.AssignedOfficer)
                .OrderByDescending(p => p.SubmittedAt)
                .ToListAsync();

        public async Task<List<Petition>> GetAssignedToOfficerAsync(int officerId)
            => await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.ActionReports)
                .Where(p => p.AssignedOfficerId == officerId)
                .OrderByDescending(p => p.AssignedAt)
                .ToListAsync();
    }
}
