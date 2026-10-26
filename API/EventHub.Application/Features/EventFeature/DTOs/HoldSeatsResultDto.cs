namespace EventHub.Application.Features.EventFeature.DTOs;
public class HoldSeatsResultDto
{
    public Guid BookingId { get; set; }

    public DateTime ExpiresAt { get; set; }
}