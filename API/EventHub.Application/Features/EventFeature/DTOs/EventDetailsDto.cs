using EventHub.Domain.Enums;
namespace EventHub.Application.Features.EventFeature.DTOs;
public class EventDetailsDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string VenueName { get; set; }

    public string City { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public List<CategoryPriceDto> Prices { get; set; }

    public List<CategorySeatAvailabilityDto> SeatsRemaining { get; set; }
}