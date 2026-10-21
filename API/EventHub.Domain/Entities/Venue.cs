using System.ComponentModel.DataAnnotations;
namespace EventHub.Domain.Entities;
public class Venue 
{
    [Required]
    public Guid Id{get;set;}
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
    [Required]
    public bool IsActive { get; set; } = true;
    public ICollection<VenueSection> Sections { get; set; }
        = new List<VenueSection>();

}
