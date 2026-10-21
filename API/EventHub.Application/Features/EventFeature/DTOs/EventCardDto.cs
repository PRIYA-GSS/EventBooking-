namespace EventHub.Application.Features.EventFeature.DTOs;
public class EventCardDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public DateTime StartTime { get; set; }

    public string VenueName { get; set; }

    public string City { get; set; }

    public decimal StartingPrice { get; set; }

    public bool IsSoldOut { get; set; }
}