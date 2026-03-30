using System.ComponentModel.DataAnnotations;

namespace PMS.Web.Models.ViewModels
{
    public class PetitionSubmitViewModel
    {
        // ── Complainant (optional) ────────────────────────────
        [Display(Name = "Complainant Name")]
        [MaxLength(200)]
        public string? ComplainantName { get; set; }

        [Display(Name = "House No / Bldg / Apt")]
        public string? HouseNo { get; set; }

        [Display(Name = "Street / Road / Lane")]
        public string? Street { get; set; }

        [Display(Name = "Area / Locality / City")]
        public string? Area { get; set; }

        [Display(Name = "District")]
        public int? ComplainantDistrictId { get; set; }

        [Display(Name = "Taluk")]
        public int? ComplainantTalukId { get; set; }

        [Display(Name = "Village / Town")]
        public string? ComplainantVillage { get; set; }

        [Display(Name = "Post Office Code")]
        public string? ComplainantPincode { get; set; }

        // ── Complaint Details ─────────────────────────────────
        [Required(ErrorMessage = "Please select a complaint category")]
        [Display(Name = "Complaint Pertaining To")]
        public int ComplaintCategoryId { get; set; }

        [Required(ErrorMessage = "Please describe your complaint")]
        [Display(Name = "Description of Complaint")]
        [MinLength(20, ErrorMessage = "Please provide at least 20 characters")]
        public string Description { get; set; } = string.Empty;

        // ── Incident Location ─────────────────────────────────
        [Required(ErrorMessage = "Please select the incident district")]
        [Display(Name = "District")]
        public int LocationDistrictId { get; set; }

        [Display(Name = "Taluk")]
        public int? LocationTalukId { get; set; }

        [Display(Name = "Village / Town")]
        public string? LocationVillage { get; set; }

        [Display(Name = "Post Office Code")]
        public string? LocationPincode { get; set; }

        // ── Contact ───────────────────────────────────────────
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "OTP is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits")]
        public string Otp { get; set; } = string.Empty;

        // ── Attachments ───────────────────────────────────────
        [Display(Name = "Documents (PDF, max 3, up to 200KB each)")]
        public List<IFormFile>? Documents { get; set; }

        [Display(Name = "Photos (JPG/JPEG, max 3, up to 200KB each)")]
        public List<IFormFile>? Photos { get; set; }
    }

    public class TrackPetitionViewModel
    {
        [Required(ErrorMessage = "Petition ID is required")]
        [Display(Name = "Petition Application ID")]
        public string ApplicationId { get; set; } = string.Empty;
    }
}
