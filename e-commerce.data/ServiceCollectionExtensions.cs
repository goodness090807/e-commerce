using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace e_commerce.data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMySqlApplicationDbContext(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "沒有設定連線字串哦");
            }

            services.AddDbContext<ApplicationDbContext>(optnios =>
            {
                optnios.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 31)));
            });

            return services;
        }
    }
}
