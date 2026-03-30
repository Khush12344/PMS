namespace PMS.Web.Helpers
{
    public static class FileUploadHelper
    {
        private static readonly string[] AllowedDocExtensions = { ".pdf" };
        private static readonly string[] AllowedPhotoExtensions = { ".jpg", ".jpeg" };

        public static bool IsValidDocument(IFormFile file, int maxSizeKB = 200)
        {
            if (file == null || file.Length == 0) return false;
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return AllowedDocExtensions.Contains(ext) && file.Length <= maxSizeKB * 1024;
        }

        public static bool IsValidPhoto(IFormFile file, int maxSizeKB = 200)
        {
            if (file == null || file.Length == 0) return false;
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return AllowedPhotoExtensions.Contains(ext) && file.Length <= maxSizeKB * 1024;
        }

        public static string GetStoredFileName(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return $"{Guid.NewGuid()}{ext}";
        }

        public static async Task<string> SaveFileAsync(IFormFile file, string uploadFolder, string storedFileName)
        {
            Directory.CreateDirectory(uploadFolder);
            var filePath = Path.Combine(uploadFolder, storedFileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return filePath;
        }
    }
}
