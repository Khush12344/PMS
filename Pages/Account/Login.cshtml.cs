using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Enums;
using PMS.Web.Services;
using System.Security.Claims;

namespace PMS.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IOtpService _otpService;
        private readonly INotificationService _notification;
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public LoginModel(IOtpService otpService, INotificationService notification,
            AppDbContext db, IConfiguration config)
        {
            _otpService   = otpService;
            _notification = notification;
            _db           = db;
            _config       = config;
        }

        [BindProperty]
        public string MobileNumber { get; set; } = string.Empty;

        [BindProperty]
        public string Otp { get; set; } = string.Empty;

        [BindProperty]
        public bool OtpSent { get; set; } = false;

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        public bool IsMockOtp => _config.GetValue<bool>("AppSettings:MockOtp");

        public void OnGet() { }

        // ── Send OTP ──────────────────────────────────────
        public async Task<IActionResult> OnPostSendOtpAsync()
        {
            if (string.IsNullOrWhiteSpace(MobileNumber) || MobileNumber.Length != 10)
            {
                ErrorMessage = "Please enter a valid 10-digit mobile number.";
                return Page();
            }

            // Only registered staff can login (not public users)
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.MobileNumber == MobileNumber &&
                u.Role != UserRole.Public &&
                u.IsActive);

            if (user == null)
            {
                ErrorMessage = "Mobile number not registered. Please contact administrator.";
                return Page();
            }

            // Generate and send OTP
            string otp = await _otpService.GenerateAndSaveOtpAsync(MobileNumber);
            await _notification.SendOtpAsync(MobileNumber, otp);

            OtpSent       = true;
            SuccessMessage = IsMockOtp
                ? "OTP sent! (Dev mode: use 123456)"
                : $"OTP sent to {MobileNumber}. Valid for 10 minutes.";

            return Page();
        }

        // ── Verify OTP and Login ──────────────────────────
        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(MobileNumber) || string.IsNullOrWhiteSpace(Otp))
            {
                ErrorMessage = "Please enter mobile number and OTP.";
                OtpSent      = true;
                return Page();
            }

            // Validate OTP
            bool valid = await _otpService.ValidateOtpAsync(MobileNumber, Otp);
            if (!valid)
            {
                ErrorMessage = "Invalid or expired OTP. Please try again.";
                OtpSent      = true;
                return Page();
            }

            // Get user details
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.MobileNumber == MobileNumber && u.IsActive);

            if (user == null)
            {
                ErrorMessage = "User not found.";
                return Page();
            }

            // Create claims — same as setting up Spring Security context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,           user.Name ?? user.MobileNumber),
                new Claim(ClaimTypes.MobilePhone,    user.MobileNumber),
                new Claim(ClaimTypes.Role,           user.Role.ToString()),
                new Claim("UserId",                  user.UserId.ToString()),
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            // Redirect based on role
            return user.Role switch
            {
                UserRole.APCCF           => RedirectToPage("/APCCF/Dashboard"),
                UserRole.Manager         => RedirectToPage("/Manager/Dashboard"),
                UserRole.OfficeAssistant => RedirectToPage("/OfficeAssistant/Dashboard"),
                UserRole.CircleOfficer   => RedirectToPage("/FieldOfficer/Dashboard"),
                UserRole.DivisionOfficer => RedirectToPage("/FieldOfficer/Dashboard"),
                UserRole.FMSOfficer      => RedirectToPage("/FieldOfficer/Dashboard"),
                _                        => RedirectToPage("/Public/SubmitComplaint")
            };
        }
    }
}
