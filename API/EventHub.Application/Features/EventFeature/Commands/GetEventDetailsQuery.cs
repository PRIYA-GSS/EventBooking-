namespace EventHub.Application.Features.EventFeature.Commands;
public record GetEventDetailsQuery(
    Guid EventId);