using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace PMS.Web.Services
{
    public class OtpService : IOtpService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<OtpService> _logger;

        public OtpService(AppDbContext db, IConfiguration config, ILogger<OtpService> logger)
        {
            _db = db;
            _config = config;
            _logger = logger;
        }

        public async Task<string> GenerateAndSaveOtpAsync(string mobileNumber)
        {
            bool mockOtp = _config.GetValue<bool>("AppSettings:MockOtp");
            string otp = mockOtp
                ? _config.GetValue<string>("AppSettings:MockOtpValue") ?? "123456"
                : GenerateOtp();

            int expiryMinutes = _config.GetValue<int>("AppSettings:OtpExpiryMinutes");
            string otpHash = HashOtp(otp);

            // Create user record if not exists (public users created on first OTP request)
            var user = await _db.Users.FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);
            if (user == null)
            {
                user = new Models.Entities.User
                {
                    MobileNumber = mobileNumber,
                    Role = Models.Enums.UserRole.Public,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                _db.Users.Add(user);
            }

            user.OtpHash = otpHash;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(expiryMinutes);
            await _db.SaveChangesAsync();

            if (mockOtp)
                _logger.LogInformation("[MOCK OTP] Mobile: {Mobile} | OTP: {Otp}", mobileNumber, otp);

            return otp;
        }

        public async Task<bool> ValidateOtpAsync(string mobileNumber, string otp)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);
            if (user == null || user.OtpHash == null || user.OtpExpiry == null)
                return false;

            if (DateTime.UtcNow > user.OtpExpiry)
                return false;

            bool valid = user.OtpHash == HashOtp(otp);

            if (valid)
            {
                // Clear OTP after successful validation
                user.OtpHash = null;
                user.OtpExpiry = null;
                await _db.SaveChangesAsync();
            }

            return valid;
        }

        private static string GenerateOtp()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4];
            rng.GetBytes(bytes);
            int value = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;
            return value.ToString("D6");
        }

        private static string HashOtp(string otp)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(otp));
            return Convert.ToHexString(bytes);
        }
    }
}
