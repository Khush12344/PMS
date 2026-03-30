using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Enums;
using PMS.Web.Services;
using System.Security.Claims;

namespace PMS.Web.Pages.Portal
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IOtpService _otpService;
        private readonly INotificationService _notification;
        private readonly IConfiguration _config;

        public LoginModel(AppDbContext db, IOtpService otpService,
            INotificationService notification, IConfiguration config)
        {
            _db           = db;
            _otpService   = otpService;
            _notification = notification;
            _config       = config;
        }

        [BindProperty] public string MobileNumber { get; set; } = string.Empty;
        [BindProperty] public string Otp { get; set; } = string.Empty;
        [BindProperty] public bool OtpSent { get; set; } = false;

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        public bool IsMockOtp => _config.GetValue<bool>("AppSettings:MockOtp");

        public void OnGet() { }

        // ── Step 1: Send OTP ──────────────────────────────────
        public async Task<IActionResult> OnPostSendOtpAsync()
        {
            if (string.IsNullOrWhiteSpace(MobileNumber) || MobileNumber.Length != 10)
            {
                ErrorMessage = "Please enter a valid 10-digit mobile number.";
                return Page();
            }

            // Check if registered
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.MobileNumber == MobileNumber &&
                u.Role == UserRole.Public &&
                u.IsActive);

            if (user == null)
            {
                ErrorMessage = "Mobile number not registered. Please register first.";
                return Page();
            }

            // Generate OTP and send SMS
            string otp = await _otpService.GenerateAndSaveOtpAsync(MobileNumber);
            await _notification.SendOtpAsync(MobileNumber, otp);

            OtpSent        = true;
            SuccessMessage = IsMockOtp
                ? "OTP sent! Dev mode — use 123456"
                : $"OTP sent to {MobileNumber}. Valid for 10 minutes.";

            return Page();
        }

        // ── Resend OTP ────────────────────────────────────────
        public async Task<IActionResult> OnPostResendOtpAsync()
        {
            if (string.IsNullOrWhiteSpace(MobileNumber))
            {
                ErrorMessage = "Mobile number missing. Please try again.";
                return Page();
            }

            string otp = await _otpService.GenerateAndSaveOtpAsync(MobileNumber);
            await _notification.SendOtpAsync(MobileNumber, otp);

            OtpSent        = true;
            SuccessMessage = "OTP resent successfully.";
            return Page();
        }

        // ── Step 2: Verify OTP and Login ──────────────────────
        public async Task<IActionResult> OnPostVerifyOtpAsync()
        {
            if (string.IsNullOrWhiteSpace(Otp) || Otp.Length != 6)
            {
                ErrorMessage = "Please enter the 6-digit OTP.";
                OtpSent      = true;
                return Page();
            }

            bool valid = await _otpService.ValidateOtpAsync(MobileNumber, Otp);
            if (!valid)
            {
                ErrorMessage = "Invalid or expired OTP. Please try again.";
                OtpSent      = true;
                return Page();
            }

            // Get user
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.MobileNumber == MobileNumber && u.IsActive);

            if (user == null)
            {
                ErrorMessage = "User not found. Please register.";
                return Page();
            }

            // Set login cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,        user.Name ?? user.MobileNumber),
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber),
                new Claim(ClaimTypes.Role,        user.Role.ToString()),
                new Claim("UserId",               user.UserId.ToString()),
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            return RedirectToPage("/Portal/Dashboard");
        }
    }
}
