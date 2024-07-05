using Microsoft.Extensions.Caching.Distributed;

namespace e_commerce.Service.Utils.TokenBlacklist
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IDistributedCache _cache;

        public TokenBlacklistService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task BlacklistTokenAsync(string token, TimeSpan? expiryTime = null)
        {
            await _cache.SetStringAsync(
                $"blacklist_{token}",
                "revoked",
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiryTime ?? TimeSpan.FromMinutes(30)
                });
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return (await _cache.GetStringAsync($"blacklist_{token}")) != null;
        }
    }
}
