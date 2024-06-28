using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace e_commerce.service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAllCustomService();

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
            var ibaseServices = assembly.GetTypes()
                .Where(type => typeof(IBaseService).IsAssignableFrom(type))
                .Where(type => type != typeof(IBaseService));

            foreach (Type iservice in ibaseServices)
            {
                if (!iservice.IsInterface)
                    continue;

                var implementInstance = ibaseServices.FirstOrDefault(type => iservice.IsAssignableFrom(type) && !type.IsInterface);

                if (implementInstance == null)
                {
                    throw new NotImplementedException($"找不到{iservice.Name}的實作");
                }

                services.AddScoped(iservice, implementInstance);
            }

            return services;
        }
    }
}
