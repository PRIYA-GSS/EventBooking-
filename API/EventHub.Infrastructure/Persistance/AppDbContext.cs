
using EventHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventHub.Infrastructure.Persistance.Configurations;
namespace EventHub.Infrastructure.Persistance;
using EventHub.Domain.Entities;
public class EventHubDbContext: IdentityDbContext<ApplicationUser>
{
    public EventHubDbContext(DbContextOptions<EventHubDbContext> options):base(options){}

    public DbSet<ApplicationUser> ApplicationUser {get;set;}
    public DbSet<RefreshToken> RefreshTokens {get;set;}
    public DbSet<Venue> Venues{get;set;}
    public DbSet<VenueSection> VenueSections{get;set;}
    public DbSet<Seat> Seats{get;set;}
    public DbSet<EventPrice> EventPrice{get;set;}
    public DbSet<EventSeat> EventSeats{get;set;}

    public DbSet<Event> Events{get;set;}
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
          builder.ApplyConfiguration(new ApplicationUserConfigurations());
          builder.ApplyConfiguration(new RefreshTokenConfiguration());
          builder.ApplyConfiguration(new VenueConfigurations());
    }
}