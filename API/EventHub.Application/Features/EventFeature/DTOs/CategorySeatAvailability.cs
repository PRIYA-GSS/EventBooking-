using EventHub.Domain.Enums;
namespace EventHub.Application.Features.EventFeature.DTOs;
public class CategorySeatAvailabilityDto
{
    public SeatCategory Category { get; set; }

    public int RemainingSeats { get; set; }
}