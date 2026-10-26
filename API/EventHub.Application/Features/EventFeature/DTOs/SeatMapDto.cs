using EventHub.Domain.Enums;
public class SeatMapDto
{
    public Guid EventSeatId { get; set; }

    public string Label { get; set; }

    public int GridX { get; set; }

    public int GridY { get; set; }

    public SeatCategory Category { get; set; }

    public SeatStatus Status { get; set; }
}