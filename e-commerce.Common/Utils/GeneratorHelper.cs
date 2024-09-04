using System.Security.Cryptography;

namespace e_commerce.Common.Utils
{
    public class GeneratorHelper
    {
        public static string GenerateRandomKey(int keySize = 32)
        {
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[keySize]; // 32 bytes will give us 256 bits.
                randomNumberGenerator.GetBytes(randomBytes);
                // Convert to Base64 for easier storage and readability
                return Convert.ToBase64String(randomBytes);
            }
        }

        public static string GetRandomStringByTime(int randomValue)
        {
            return DateTime.UtcNow.AddHours(8).ToString("yyyyMMddHHmmssfff") + new Random().Next(randomValue);
        }
    }
}
