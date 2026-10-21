using EventHub.Domain.Enums;
namespace EventHub.Domain.Entities;
public class EventPrice
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }

    public SeatCategory Category { get; set; }

    public decimal Price { get; set; }
}