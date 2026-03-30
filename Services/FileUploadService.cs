using PMS.Data;
using PMS.Web.Helpers;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;

namespace PMS.Web.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public FileUploadService(AppDbContext db, IWebHostEnvironment env, IConfiguration config)
        {
            _db = db;
            _env = env;
            _config = config;
        }

        public async Task<List<string>> SavePetitionAttachmentsAsync(int petitionId, List<IFormFile> documents, List<IFormFile> photos)
        {
            var saved = new List<string>();
            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "petitions", petitionId.ToString());

            foreach (var doc in documents.Where(f => f != null && f.Length > 0))
            {
                string storedName = FileUploadHelper.GetStoredFileName(doc);
                await FileUploadHelper.SaveFileAsync(doc, uploadFolder, storedName);
                _db.PetitionAttachments.Add(new PetitionAttachment
                {
                    PetitionId       = petitionId,
                    FileType         = AttachmentType.Document,
                    StoredFileName   = storedName,
                    OriginalFileName = doc.FileName,
                    FileSizeBytes    = doc.Length,
                    UploadedAt       = DateTime.UtcNow
                });
                saved.Add(storedName);
            }

            foreach (var photo in photos.Where(f => f != null && f.Length > 0))
            {
                string storedName = FileUploadHelper.GetStoredFileName(photo);
                await FileUploadHelper.SaveFileAsync(photo, uploadFolder, storedName);
                _db.PetitionAttachments.Add(new PetitionAttachment
                {
                    PetitionId       = petitionId,
                    FileType         = AttachmentType.Photo,
                    StoredFileName   = storedName,
                    OriginalFileName = photo.FileName,
                    FileSizeBytes    = photo.Length,
                    UploadedAt       = DateTime.UtcNow
                });
                saved.Add(storedName);
            }

            await _db.SaveChangesAsync();
            return saved;
        }

        public async Task<List<string>> SaveReportAttachmentsAsync(int reportId, List<IFormFile> documents, List<IFormFile> photos)
        {
            var saved = new List<string>();
            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "reports", reportId.ToString());

            foreach (var doc in documents.Where(f => f != null && f.Length > 0))
            {
                string storedName = FileUploadHelper.GetStoredFileName(doc);
                await FileUploadHelper.SaveFileAsync(doc, uploadFolder, storedName);
                _db.ActionReportAttachments.Add(new ActionReportAttachment
                {
                    ReportId         = reportId,
                    FileType         = AttachmentType.Document,
                    StoredFileName   = storedName,
                    OriginalFileName = doc.FileName,
                    FileSizeBytes    = doc.Length
                });
                saved.Add(storedName);
            }

            foreach (var photo in photos.Where(f => f != null && f.Length > 0))
            {
                string storedName = FileUploadHelper.GetStoredFileName(photo);
                await FileUploadHelper.SaveFileAsync(photo, uploadFolder, storedName);
                _db.ActionReportAttachments.Add(new ActionReportAttachment
                {
                    ReportId         = reportId,
                    FileType         = AttachmentType.Photo,
                    StoredFileName   = storedName,
                    OriginalFileName = photo.FileName,
                    FileSizeBytes    = photo.Length
                });
                saved.Add(storedName);
            }

            await _db.SaveChangesAsync();
            return saved;
        }

        // APCCF optional single PDF upload
        public async Task<string> SaveAPCCFDocumentAsync(int petitionId, IFormFile document)
        {
            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "apccf", petitionId.ToString());
            string storedName   = FileUploadHelper.GetStoredFileName(document);
            await FileUploadHelper.SaveFileAsync(document, uploadFolder, storedName);
            return $"uploads/apccf/{petitionId}/{storedName}";
        }
    }
}
