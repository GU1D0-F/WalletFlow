using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalletFlow.Application.Repositories.Core;
using WalletFlow.Infrastructure;
using WalletFlow.Infrastructure.Repositories.Core;

namespace WalletFlow.IoC;

public static class Infrastructure
{
    public static IServiceCollection AddWalletFlowInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddNpgsqlContext<CoreDbContext>(configuration.GetConnectionString("Default")!)
                .AddRepositories();
        
        return services;
    }
    
    private static IServiceCollection AddNpgsqlContext<T>(this IServiceCollection services, string connectionString) where T : DbContext
    {
        services.AddDbContext<T>(options =>
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IWalletRepository, WalletRepository>()
                       .AddScoped<IWalletEntryRepository, WalletEntryRepository>();
    }
}