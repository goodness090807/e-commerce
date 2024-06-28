namespace e_commerce.Common.Utils
{
    public class HashHelper
    {
        // 生成密碼哈希
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // 驗證密碼
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
