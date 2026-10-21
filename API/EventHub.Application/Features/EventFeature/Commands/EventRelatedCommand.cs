using EventHub.Application.Features.EventFeature.DTOs;
namespace EventHub.Application.Features.EventFeature.Commands;
public record SetPricesCommand(
    SetPricesDto Dto,
    string OrganizerId);
public record PublishEventCommand(
    Guid EventId,
    string OrganizerId);
public record GetMyEventsQuery(
    string OrganizerId);

public record UpdatePublishedEventCommand(
    UpdateEventDto Dto,
    string OrganizerId);
public record UpdateDraftEventCommand(
    UpdateDraftEvent Dto,
    string OrganizerId);

public record CancelEventCommand(
    Guid EventId,
    string OrganizerId);

public record BlockSeatsCommand(
    BlockSeatsDto Dto,
    string OrganizerId);