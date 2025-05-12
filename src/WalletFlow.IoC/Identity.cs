using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WalletFlow.Application.Services.Identity.JwtToken;
using WalletFlow.Domain.Entities.Users;
using WalletFlow.Infrastructure;

namespace WalletFlow.IoC;

public static class Identity
{
    public static IServiceCollection AddWalletFlowIdentity(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .ConfigureIdentity()
            .ConfigureJwtAuthentication(configuration)
            .AddAuthorization()
            .AddHttpContextAccessor()
            .AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
    
    private static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                
                //Minimizando complexidade de senha
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<CoreDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"]!;
        var jwtIssuer = configuration["Jwt:Issuer"]!;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = key,
                    ValidateIssuerSigningKey = true
                };
            });

        return services;
    }
}