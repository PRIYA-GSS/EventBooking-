namespace EventHub.Application.Features.EventFeature.DTOs;
public class HoldSeatsDto
{
    public Guid EventId { get; set; }

    public List<Guid> EventSeatIds { get; set; }
}