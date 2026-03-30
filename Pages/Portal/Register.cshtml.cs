using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IOtpService _otpService;
        private readonly INotificationService _notification;
        private readonly IConfiguration _config;

        public RegisterModel(AppDbContext db, IOtpService otpService,
            INotificationService notification, IConfiguration config)
        {
            _db = db;
            _otpService = otpService;
            _notification = notification;
            _config = config;
        }

        [BindProperty] public string Name { get; set; } = string.Empty;
        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? HouseNo { get; set; }
        [BindProperty] public string? Street { get; set; }
        [BindProperty] public string? Area { get; set; }
        [BindProperty] public int DistrictId { get; set; }
        [BindProperty] public int? TalukId { get; set; }
        [BindProperty] public string? Village { get; set; }
        [BindProperty] public string? Pincode { get; set; }
        [BindProperty] public string MobileNumber { get; set; } = string.Empty;
        [BindProperty] public string? Otp { get; set; }
        [BindProperty] public bool OtpSent { get; set; } = false;

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        public bool IsMockOtp => _config.GetValue<bool>("AppSettings:MockOtp");

        public List<District> Districts { get; set; } = new();
        public List<Taluk> Taluks { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadMasterDataAsync();
        }

        // ── Send OTP ──────────────────────────────────────────
        public async Task<IActionResult> OnPostSendOtpAsync()
        {
            await LoadMasterDataAsync();

            if (string.IsNullOrWhiteSpace(MobileNumber) || MobileNumber.Length != 10)
            {
                ErrorMessage = "Please enter a valid 10-digit mobile number.";
                return Page();
            }

            // Check if already fully registered (Name is not null means fully registered)
            var existing = await _db.Users.FirstOrDefaultAsync(u =>
                u.MobileNumber == MobileNumber && u.Name != null);

            if (existing != null)
            {
                ErrorMessage = "Mobile number already registered. Please login.";
                return Page();
            }

            string otp = await _otpService.GenerateAndSaveOtpAsync(MobileNumber);
            await _notification.SendOtpAsync(MobileNumber, otp);

            OtpSent = true;
            SuccessMessage = IsMockOtp ? "OTP sent! Dev mode — use 123456" : $"OTP sent to {MobileNumber}.";
            return Page();
        }

        // ── Register ──────────────────────────────────────────
        public async Task<IActionResult> OnPostRegisterAsync()
        {
            await LoadMasterDataAsync();

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Full name is required.";
                OtpSent = true;
                return Page();
            }

            if (string.IsNullOrWhiteSpace(MobileNumber) || MobileNumber.Length != 10)
            {
                ErrorMessage = "Please enter a valid 10-digit mobile number.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Otp))
            {
                ErrorMessage = "Please enter the OTP sent to your mobile.";
                OtpSent = true;
                return Page();
            }

            // Validate OTP
            bool valid = await _otpService.ValidateOtpAsync(MobileNumber, Otp);
            if (!valid)
            {
                ErrorMessage = "Invalid or expired OTP. Please try again.";
                OtpSent = true;
                return Page();
            }

            // Find existing temp record created by OtpService when OTP was sent
            var existingUser = await _db.Users.FirstOrDefaultAsync(u =>
                u.MobileNumber == MobileNumber);

            User user;

            if (existingUser != null)
            {
                // Update temp record with full registration details
                existingUser.Name = Name;
                existingUser.Email = string.IsNullOrWhiteSpace(Email) ? null : Email;
                existingUser.HouseNo = HouseNo;
                existingUser.Street = Street;
                existingUser.Area = Area;
                existingUser.DistrictId = DistrictId > 0 ? DistrictId : null;
                existingUser.TalukId = TalukId;
                existingUser.Village = Village;
                existingUser.Pincode = Pincode;
                existingUser.IsActive = true;
                await _db.SaveChangesAsync();
                user = existingUser;
            }
            else
            {
                // No temp record — create fresh
                user = new User
                {
                    MobileNumber = MobileNumber,
                    Name = Name,
                    Email = string.IsNullOrWhiteSpace(Email) ? null : Email,
                    HouseNo = HouseNo,
                    Street = Street,
                    Area = Area,
                    DistrictId = DistrictId > 0 ? DistrictId : null,
                    TalukId = TalukId,
                    Village = Village,
                    Pincode = Pincode,
                    Role = UserRole.Public,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            // Sign in after registration
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,        user.Name ?? user.MobileNumber),
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber),
                new Claim(ClaimTypes.Role,        user.Role.ToString()),
                new Claim("UserId",               user.UserId.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            return RedirectToPage("/Portal/Dashboard");
        }

        private async Task LoadMasterDataAsync()
        {
            Districts = await _db.Districts.Where(d => d.IsActive).OrderBy(d => d.Name).ToListAsync();
            Taluks = await _db.Taluks.Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync();
        }
    }
}