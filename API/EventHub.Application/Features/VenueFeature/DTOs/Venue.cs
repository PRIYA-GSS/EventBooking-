using EventHub.Domain.Entities;
using  EventHub.Domain.Enums;
namespace EventHub.Application.Features.VenueFeature.DTOs;
public class Venue
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public List<VenueSection> sections = new List<VenueSection>();

}