using EventHub.Infrastructure.Identity;
using EventHub.Infrastructure.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EventHub.Application.Common.Interfaces;
using EventHub.Infrastructure.Identity.Jwt;
using EventHub.Infrastructure.Identity.Services;
using EventHub.Infrastructure.EventServices;
using EventHub.Infrastructure.VenueServices;

namespace EventHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //database
        services.AddDbContext<EventHubDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        //identity
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<EventHubDbContext>().AddDefaultTokenProviders();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.Configure<DataProtectionTokenProviderOptions>(
            options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(5);
            }
    );
        services.AddAutoMapper(typeof(IdentityMapping));
        services.AddAutoMapper(typeof(VenueMapping));
        services.AddAutoMapper(typeof(EventMapping));
        return services;
    }
}