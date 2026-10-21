using EventHub.Application.Features.VenueFeature.DTOs;

namespace EventHub.Application.Features.VenueFeature.Commands;
public record ActivateCommand(
    Guid VenueId,
    bool IsActive);

public record CreateLayout(
    Guid VenueId,
    List<SectionLayout> Sections); 