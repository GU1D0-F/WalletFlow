using Microsoft.AspNetCore.Builder;
using WalletFlow.Infrastructure.Middlewares;

namespace WalletFlow.IoC;

public static class Middlewares
{
    public static WebApplication UseOquesobraMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}