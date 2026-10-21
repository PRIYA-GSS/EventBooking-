using EventHub.Domain.Enums;
namespace EventHub.Domain.Entities;
public class Event
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public EventCategory Category { get; set; }

    public Guid VenueId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public EventStatus Status { get; set; }
    public Venue Venue {get;set;}

    public string OrganizerId { get; set; } = string.Empty;
}