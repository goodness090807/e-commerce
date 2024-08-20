using e_commerce.Service.Utils.TokenBlacklist;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace e_commerce.Service
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 註冊自有的服務
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAllCustomService();
            services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

            return services;
        }

        /// <summary>
        /// --------------------------
        /// 實現自動註冊自己撰寫的服務
        /// --------------------------
        /// 
        /// 此實現的邏輯為，自己寫的Interface要繼承IService介面
        /// </summary>
        public static IServiceCollection AddAllCustomService(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var baseServices = assembly.GetTypes()
                .Where(type => typeof(IBaseService).IsAssignableFrom(type))
                .Where(type => type != typeof(IBaseService))
                .Where(type => type.IsInterface);

            foreach (Type baseService in baseServices)
            {
                var implementType = Array.Find(assembly.GetTypes(), type => baseService.IsAssignableFrom(type) && !type.IsInterface);
                if (implementType == null)
                {
                    throw new NotImplementedException($"找不到{baseService.Name}的實作");
                }

                services.AddScoped(baseService, implementType);
            }

            return services;
        }
    }
}
