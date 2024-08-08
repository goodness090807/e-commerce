using e_commerce.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

namespace e_commerce.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBasicCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    // TODO： 需要把正式機和測試機拆開
                    builder.WithOrigins("https://localhost:8081", "https://localhost:7204")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string? issuer, string? audience, string? secretKey)
        {
            if (secretKey  == null)
                throw new ArgumentNullException(nameof(secretKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    };
                }
            );
            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "e-commerce", Version = "v1" });
                options.ExampleFilters();

                #region 加鎖
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                });
                options.OperationFilter<SwaggerFilters>();
                #endregion

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
            });

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            return services;
        }

        public static IServiceCollection AddStackExchangeRedis(this IServiceCollection services, string? host, string? instanceName)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = host;
                options.InstanceName = instanceName;
            });
            return services;
        }
    }
}
