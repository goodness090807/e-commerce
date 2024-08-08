using System.Security.Claims;

namespace e_commerce.Extensions
{
    public static class UserExtensions
    {
        // 一個參數，用來取得使用者的Id
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
