namespace e_commerce.Common.Models
{
    public class AppSettings
    {
        public JwtSettings Jwt { get; set; } = new JwtSettings();
        public VerificationSettings Verification { get; set; } = new VerificationSettings();
        public EmailSettings Email { get; set; } = new EmailSettings();
        public GoogleStorageSettings GoogleStorage { get; set; } = new GoogleStorageSettings();

        public class JwtSettings
        {
            public string SecretKey { get; set; } = string.Empty;
            public string Issuer { get; set; } = string.Empty;
            public string Audience { get; set; } = string.Empty;
            public int TokenExpirationMinutes { get; set; }
            public int RefreshTokenExpirationDays { get; set; }
        }

        public class VerificationSettings
        {
            public string BaseUrl { get; set; } = string.Empty;
            public int TokenExpirationMinutes { get; set; }
        }

        public class EmailSettings
        {
            public string Host { get; set; } = string.Empty;
            public int Port { get; set; }
            public string Name { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class GoogleStorageSettings
        {
            public string BucketName { get; set; } = string.Empty;
            public string BaseUrl { get; set; } = string.Empty;
        }
    }
}
