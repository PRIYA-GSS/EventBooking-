namespace EventHub.Domain.Entities;
public class BookingSeat
{
    public Guid Id { get; set; }

    public Guid BookingId { get; set; }

    public Guid EventSeatId { get; set; }

    public Booking Booking { get; set; }

    public EventSeat EventSeat { get; set; }
}