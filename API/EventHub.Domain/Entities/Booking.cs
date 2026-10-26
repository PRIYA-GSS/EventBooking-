using EventHub.Domain.Enums;

namespace EventHub.Domain.Entities;
public class Booking
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }

    public string CustomerId { get; set; }

    public BookingStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public ICollection<BookingSeat> BookingSeats { get; set; }
}