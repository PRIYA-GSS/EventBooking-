using EventHub.Domain.Enums;
namespace EventHub.Application.Features.EventFeature.DTOs;
public class SetPricesDto
{
    public Guid EventId { get; set; }

    public List<CategoryPriceDto> Prices { get; set; }
}