using EventHub.Application.Features.VenueFeature.DTOs;

public class CreateVenueLayoutRequest
{
    public Guid VenueId { get; set; }

    public List<SectionLayout> Sections { get; set; }
}