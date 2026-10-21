using EventHub.Domain.Enums;
namespace EventHub.Domain.Entities;
public class VenueSection
{
    public Guid Id { get; set; }

    public Guid VenueId { get; set; }

    public string Name { get; set; } = string.Empty;

    public SeatCategory Category { get; set; }

    public Venue Venue { get; set; }

    public ICollection<Seat> Seats { get; set; }
}