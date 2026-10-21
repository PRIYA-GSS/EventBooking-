using EventHub.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EventHub.Infrastructure.Persistance.Configurations;

public class ApplicationUserConfigurations:IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u=>u.FullName).HasMaxLength(150).IsRequired();
    }
}