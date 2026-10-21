public class BlockSeatsDto
{
    public Guid EventId { get; set; }

    public List<Guid> EventSeatIds { get; set; }
}