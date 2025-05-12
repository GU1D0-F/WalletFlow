namespace WalletFlow.Api.Commons.Extensions;

public static class Cors
{
    private static readonly string[] Origins =
    [
        "http://localhost:3333/"
    ];

    public static IServiceCollection AddFinancialManagerCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(Origins)
                    .AllowCredentials());
        });

        return services;
    }
}