using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using PMS.Web.Services;
using System.Security.Claims;

namespace PMS.Web.Pages.Portal
{
    [Authorize(Roles = "Public")]
    public class SubmitComplaintModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IPetitionService _petitionService;
        private readonly IConfiguration _config;

        public SubmitComplaintModel(AppDbContext db, IPetitionService petitionService, IConfiguration config)
        {
            _db = db;
            _petitionService = petitionService;
            _config = config;
        }

        public List<District> Districts { get; set; } = new();
        public List<ComplaintCategory> Categories { get; set; } = new();
        public string MobileNumber { get; set; } = string.Empty;
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SubmittedPetitionId { get; set; }
        public bool IsMockOtp => _config.GetValue<bool>("AppSettings:MockOtp");

        // Complainant
        [BindProperty] public string ComplainantName { get; set; } = string.Empty;
        [BindProperty] public string? HouseNo { get; set; }
        [BindProperty] public string? Street { get; set; }
        [BindProperty] public string? Area { get; set; }
        [BindProperty] public int? ComplainantDistrictId { get; set; }
        [BindProperty] public bool? ComplainantIsUrban { get; set; }
        [BindProperty] public int? ComplainantTalukId { get; set; }
        [BindProperty] public int? ComplainantHobliId { get; set; }
        [BindProperty] public int? ComplainantVillageId { get; set; }
        [BindProperty] public int? ComplainantTownId { get; set; }
        [BindProperty] public string? ComplainantPincode { get; set; }

        // Incident Location
        [BindProperty] public int LocationDistrictId { get; set; }
        [BindProperty] public bool? LocationIsUrban { get; set; }
        [BindProperty] public int? LocationTalukId { get; set; }
        [BindProperty] public int? LocationHobliId { get; set; }
        [BindProperty] public int? LocationVillageId { get; set; }
        [BindProperty] public int? LocationTownId { get; set; }
        [BindProperty] public string? LocationPincode { get; set; }

        // Complaint
        [BindProperty] public int ComplaintCategoryId { get; set; }
        [BindProperty] public string Description { get; set; } = string.Empty;

        // Files
        [BindProperty] public IFormFile? Document1 { get; set; }
        [BindProperty] public IFormFile? Document2 { get; set; }
        [BindProperty] public IFormFile? Photo1 { get; set; }
        [BindProperty] public IFormFile? Photo2 { get; set; }
        [BindProperty] public IFormFile? Photo3 { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();
            ComplainantName = User.Identity?.Name ?? "";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MobileNumber = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";
            await LoadAsync();

            if (string.IsNullOrWhiteSpace(Description) || LocationDistrictId == 0 || ComplaintCategoryId == 0)
            {
                ErrorMessage = "Please fill all required fields.";
                return Page();
            }

            try
            {
                var petition = new Petition
                {
                    PetitionApplicationId = await _petitionService.GenerateNextApplicationIdAsync(),
                    ComplainantName       = ComplainantName,
                    HouseNo               = HouseNo,
                    Street                = Street,
                    Area                  = Area,
                    ComplainantDistrictId = ComplainantDistrictId,
                    ComplainantIsUrban    = ComplainantIsUrban,
                    ComplainantTalukId    = ComplainantTalukId,
                    ComplainantHobliId    = ComplainantHobliId,
                    ComplainantVillageId  = ComplainantVillageId,
                    ComplainantTownId     = ComplainantTownId,
                    ComplainantPincode    = ComplainantPincode,
                    ComplaintCategoryId   = ComplaintCategoryId,
                    Description           = Description,
                    LocationDistrictId    = LocationDistrictId,
                    LocationIsUrban       = LocationIsUrban,
                    LocationTalukId       = LocationTalukId,
                    LocationHobliId       = LocationHobliId,
                    LocationVillageId     = LocationVillageId,
                    LocationTownId        = LocationTownId,
                    LocationPincode       = LocationPincode,
                    MobileNumber          = MobileNumber,
                    Status                = PetitionStatus.Submitted,
                    SubmittedAt           = DateTime.UtcNow
                };

                _db.Petitions.Add(petition);
                await _db.SaveChangesAsync();

                var docs   = new List<IFormFile?> { Document1, Document2 }.Where(f => f != null).Cast<IFormFile>().ToList();
                var photos = new List<IFormFile?> { Photo1, Photo2, Photo3 }.Where(f => f != null).Cast<IFormFile>().ToList();

                if (docs.Any() || photos.Any())
                {
                    var fileService = HttpContext.RequestServices.GetRequiredService<IFileUploadService>();
                    await fileService.SavePetitionAttachmentsAsync(petition.PetitionId, docs, photos);
                }

                _db.PetitionWorkflowLogs.Add(new PetitionWorkflowLog
                {
                    PetitionId  = petition.PetitionId,
                    Action      = "Submitted",
                    PublicLabel = " Petition Submitted",
                    FromStatus  = PetitionStatus.Submitted,
                    ToStatus    = PetitionStatus.Submitted,
                    ActionAt    = DateTime.UtcNow
                });
                await _db.SaveChangesAsync();

                SubmittedPetitionId = petition.PetitionApplicationId;
                SuccessMessage = $"Complaint submitted! Your ID: {petition.PetitionApplicationId}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }

            return Page();
        }

        // ── AJAX Handlers ─────────────────────────────────────────
        public async Task<IActionResult> OnGetTaluksAsync(int districtId)
        {
            var data = await _db.Taluks
                .Where(t => t.DistrictId == districtId && t.IsActive)
                .OrderBy(t => t.Name)
                .Select(t => new { talukId = t.TalukId, name = t.Name, nameKN = t.NameKN })
                .ToListAsync();
            return new JsonResult(data);
        }

        public async Task<IActionResult> OnGetHoblisAsync(int talukId)
        {
            var data = await _db.Hoblis
                .Where(h => h.TalukId == talukId)
                .OrderBy(h => h.NameEN)
                .Select(h => new { hobliId = h.HobliId, name = h.NameEN, nameKN = h.NameKN })
                .ToListAsync();
            return new JsonResult(data);
        }

        public async Task<IActionResult> OnGetVillagesAsync(int hobliId)
        {
            var data = await _db.Villages
                .Where(v => v.HobliId == hobliId)
                .OrderBy(v => v.NameEN)
                .Select(v => new { villageId = v.VillageId, name = v.NameEN, nameKN = v.NameKN })
                .ToListAsync();
            return new JsonResult(data);
        }

        public async Task<IActionResult> OnGetTownsAsync(int districtId)
        {
            var data = await _db.Towns
                .Where(t => t.DistrictId == districtId)
                .OrderBy(t => t.NameEN)
                .Select(t => new { townId = t.TownId, name = t.NameEN, t.TownType })
                .ToListAsync();
            return new JsonResult(data);
        }

        private async Task LoadAsync()
        {
            MobileNumber = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";
            Districts    = await _db.Districts.Where(d => d.IsActive).OrderBy(d => d.Name).ToListAsync();
            Categories   = await _db.ComplaintCategories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
        }
    }
}
