using Microsoft.Extensions.DependencyInjection;
using EventHub.Application.Features.Authentication.Handlers;
namespace EventHub.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<RefreshTokenHandler>();
        return services;
    }
}
