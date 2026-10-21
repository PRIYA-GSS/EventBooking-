using EventHub.Domain.Enums;

namespace EventHub.Application.Features.VenueFeature.DTOs;
public class SectionLayout
{
    public string Name { get; set; }

    public SeatCategory Category { get; set; }

    public List<string> Rows { get; set; }

    public int SeatsPerRow { get; set; }
 
}