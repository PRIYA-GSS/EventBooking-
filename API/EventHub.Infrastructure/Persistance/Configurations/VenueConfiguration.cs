using EventHub.Infrastructure.Identity;
using EventHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EventHub.Infrastructure.Persistance.Configurations;

public class VenueConfigurations:IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.HasIndex(x=> new {x.Name, x.City}).IsUnique();
        
    }
}