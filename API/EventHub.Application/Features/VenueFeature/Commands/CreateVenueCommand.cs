namespace EventHub.Application.Features.VenueFeature.Commands;
public record CreateVenueCommand(
    string Name,
    string City,
    string Address);