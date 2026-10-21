using EventHub.Domain.Enums;
namespace EventHub.Domain.Entities;
public class EventSeat
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public SeatStatus Status { get; set; }
}