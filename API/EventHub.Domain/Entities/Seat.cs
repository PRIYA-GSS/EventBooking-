namespace EventHub.Domain.Entities;
public class Seat
{
    public Guid Id { get; set; }

    public Guid SectionId { get; set; }

    public string Label { get; set; } = string.Empty;

    public int GridX { get; set; }

    public int GridY { get; set; }

    public VenueSection Section { get; set; }
}