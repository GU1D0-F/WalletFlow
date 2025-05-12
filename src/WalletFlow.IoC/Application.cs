using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WalletFlow.Application;
using WalletFlow.Application.Behaviors;

namespace WalletFlow.IoC;

public static class Application
{
    public static IServiceCollection AddWalletFlowApplication(this IServiceCollection services)
    {
        services.AddMediatR(cf =>
        {
            cf.RegisterServicesFromAssembly(typeof(ApplicationRef).Assembly);
            cf.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(typeof(ApplicationRef).Assembly);
        
        return services;
    }
}