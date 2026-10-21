using EventHub.Application.Features.EventFeature.DTOs;

namespace EventHub.Application.Features.EventFeature.Commands;
public record GetUpcomingEventsQuery(
    EventSearchDto Dto);