namespace e_commerce.Service.Utils.TokenBlacklist
{
    public interface ITokenBlacklistService
    {
        Task BlacklistTokenAsync(string token, TimeSpan? expiryTime = null);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
