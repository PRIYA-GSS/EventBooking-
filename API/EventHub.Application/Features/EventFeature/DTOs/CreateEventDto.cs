using EventHub.Domain.Enums;
namespace EventHub.Application.Features.EventFeature.DTOs;
public class CreateEventDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public EventCategory Category { get; set; }

    public Guid VenueId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
    public string OrganizerId { get; set; } = string.Empty;
}