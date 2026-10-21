using EventHub.Domain.Enums;
namespace EventHub.Application.Features.EventFeature.DTOs;
public class CategoryPriceDto
{
    public SeatCategory Category { get; set; }

    public decimal Price { get; set; }
}