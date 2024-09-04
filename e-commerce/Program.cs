using e_commerce.Middlewares;
using e_commerce.Data;
using e_commerce.Service;
using e_commerce.Extensions;
using Serilog;
using e_commerce.Common.Models;

var builder = WebApplication.CreateBuilder(args);
// UseSentry
builder.Host.UseSerilog((_, c) =>
{
    c.Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Sentry();
});
builder.WebHost.UseSentry();


// Add services to the container.
var configuration = builder.Configuration;
var env = builder.Environment.EnvironmentName;
configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
builder.Services.Configure<AppSettings>(configuration);

builder.Services.AddMySqlApplicationDbContext(configuration.GetConnectionString("MySql") ?? "")
    .AddServices(builder.Environment)
    .AddBasicCors()
    .AddJwtAuthentication(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], configuration["Jwt:SecretKey"])
    .AddAuthorizationPolicies()
    .AddStackExchangeRedis(configuration["Redis:Host"], configuration["Redis:InstanceName"])
    .AddControllers();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<TokenValidationMiddleware>();

// 需要在 UseAuthorization 之前
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
