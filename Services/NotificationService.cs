using System.Security.Cryptography;
using System.Text;

namespace PMS.Web.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<NotificationService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        // SMS Gateway URL (from Next.js implementation)
        private const string SmsApiUrl = "http://65.2.76.193/index.php/sendmsg";
        private const string AppName   = "Petition Management System";

        public NotificationService(IConfiguration config, ILogger<NotificationService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _config            = config;
            _logger            = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendOtpAsync(string mobileNumber, string otp)
        {
            bool mockOtp = _config.GetValue<bool>("AppSettings:MockOtp");

            if (mockOtp)
            {
                _logger.LogInformation("[MOCK OTP] Mobile: {Mobile} | OTP: {Otp}", mobileNumber, otp);
                return;
            }

            // DLT-compliant message — DO NOT change this format
            string message = $"Your One Time Password (OTP) for request in {AppName} is {otp}. From KFDICT";
            await SendSmsAsync(mobileNumber, message);
        }

        public async Task SendPetitionAcknowledgementAsync(string mobileNumber, string applicationId)
        {
            bool mockOtp = _config.GetValue<bool>("AppSettings:MockOtp");

            if (mockOtp)
            {
                _logger.LogInformation("[MOCK SMS] Mobile: {Mobile} | Petition ID: {AppId}", mobileNumber, applicationId);
                return;
            }

            string message = $"Your complaint has been submitted successfully on {AppName}. Your Petition ID is {applicationId}. Use this ID to track your complaint. From KFDICT";
            await SendSmsAsync(mobileNumber, message);
        }

        public async Task SendSmsAsync(string mobileNumber, string message)
        {
            try
            {
                // Read credentials from appsettings (same as Next.js process.env)
                string username      = _config["SmsSettings:Username"]!;
                string password      = _config["SmsSettings:Password"]!;
                string senderid      = _config["SmsSettings:SenderId"]!;
                string deptSecureKey = _config["SmsSettings:SecureKey"]!;
                string templateid    = _config["SmsSettings:TemplateId"]!;

                // ── Step 1: Encrypt password with SHA1 ───────────────
                // Same as: crypto.createHash('sha1').update(password).digest('hex')
                string encryptedPassword = ComputeSha1(password);

                // ── Step 2: Generate secure key with SHA512 ───────────
                // Same as: crypto.createHash('sha512').update(username + senderid + message + deptSecureKey).digest('hex')
                string key = ComputeSha512(username + senderid + message + deptSecureKey);

                // ── Step 3: Build payload ─────────────────────────────
                // Field names exactly as in Next.js payload
                var parameters = new Dictionary<string, string>
                {
                    { "username",       username          },
                    { "password",       encryptedPassword },
                    { "senderid",       senderid          },
                    { "content",        message           },
                    { "smsservicetype", "otpmsg"          },
                    { "mobileno",       mobileNumber      },
                    { "key",            key               },
                    { "templateid",     templateid        }
                };

                // ── Step 4: POST to SMS gateway ───────────────────────
                var client   = _httpClientFactory.CreateClient("SmsClient");
                var content  = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync(SmsApiUrl, content);
                var result   = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("SMS Gateway Response for {Mobile}: {Result}", mobileNumber, result);
            }
            catch (Exception ex)
            {
                // Never crash the petition flow due to SMS failure
                _logger.LogError(ex, "SMS exception for {Mobile}", mobileNumber);
            }
        }

        // ── Hashing Helpers ───────────────────────────────────────────

        /// SHA1 — same as crypto.createHash('sha1').update(input).digest('hex')
        private static string ComputeSha1(string input)
        {
            byte[] bytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLower();
        }

        /// SHA512 — same as crypto.createHash('sha512').update(input).digest('hex')
        private static string ComputeSha512(string input)
        {
            byte[] bytes = SHA512.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
