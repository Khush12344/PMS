namespace PMS.Web.Helpers
{
    public class AppSettings
    {
        public int OtpExpiryMinutes { get; set; } = 10;
        public bool MockOtp { get; set; } = false;
        public string MockOtpValue { get; set; } = "123456";
        public string UploadPath { get; set; } = "wwwroot/uploads";
        public int MaxDocumentSizeKB { get; set; } = 200;
        public int MaxPhotoSizeKB { get; set; } = 200;
        public int MaxDocumentsPerPetition { get; set; } = 3;
        public int MaxPhotosPerPetition { get; set; } = 3;
    }
}
