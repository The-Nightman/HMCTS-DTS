using HmctsDts.Server.Data;
using HmctsDts.Server.Data.Repositories;
using HmctsDts.Server.Interfaces;
using HmctsDts.Server.Security;
using HmctsDts.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace HmctsDts.Server.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors();
        services.AddControllers();
        services.AddDbContext<DataContext>(opt => { opt.UseNpgsql(config.GetConnectionString("DefaultConnection")); });
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountsService, AccountsService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddSingleton<byte[]>(prov =>
        {
            var pepperString = config["Security:Pepper"];
            if (string.IsNullOrEmpty(pepperString))
            {
                throw new InvalidOperationException("Pepper value is not set in the configuration.");
            }

            return System.Text.Encoding.UTF8.GetBytes(pepperString);
        });

        return services;
    }
}