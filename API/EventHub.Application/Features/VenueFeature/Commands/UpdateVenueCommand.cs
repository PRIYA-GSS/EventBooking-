namespace EventHub.Application.Features.VenueFeature.Commands;
public record UpdateVenueCommand(
    Guid Id,
    string Name,
    string City,
    string Address
);
public record GetVenueByIdQuery(
    Guid Id
);

public record GetAllVenuesQuery(string? search,
        string? city,
        int page,
        int pageSize);