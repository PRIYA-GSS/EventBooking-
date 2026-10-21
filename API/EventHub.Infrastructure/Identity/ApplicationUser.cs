using Microsoft.AspNetCore.Identity;
namespace EventHub.Infrastructure.Identity;
public class ApplicationUser:IdentityUser
{
    public string FullName  { get; set; }
    public DateTime CreatedAt { get; set; }
}