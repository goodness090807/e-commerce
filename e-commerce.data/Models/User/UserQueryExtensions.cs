using Microsoft.EntityFrameworkCore;

namespace e_commerce.Data.Models.User
{
    public static class UserQueryExtensions
    {
        public static async Task<UserModel?> GetValidUserAsync(this DbSet<UserModel> users, int userId)
        {
            return await users.FirstOrDefaultAsync(x => x.Id == userId && x.Valid);
        }
        public static async Task<bool> CheckValidUserAsync(this DbSet<UserModel> users, int userId)
        {
            return await users.AnyAsync(x => x.Id == userId && x.Valid);
        }
    }
}
