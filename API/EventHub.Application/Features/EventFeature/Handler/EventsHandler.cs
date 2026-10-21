using EventHub.Application.Features.EventFeature.Interface;
using EventHub.Application.Common.Models;
using EventHub.Application.Features.EventFeature.DTOs;
using EventHub.Application.Features.EventFeature.Commands;
namespace EventHub.Application.Features.EventFeature.Handler;

public class EventsHandler
{
    private readonly IEventService _service;

    public EventsHandler(IEventService service)
    {
        _service = service;
    }

    public async Task<Result> Handle(
        CreateEventCommand command)
    {
        return await _service.CreateEventAsync(
           command.dto);
    }
    public async Task<Result> Handle(SetPricesCommand command)
    {
        return await _service.SetPricesAsync(
            command.Dto,
            command.OrganizerId);
    }

    // US-303
    public async Task<Result> Handle(PublishEventCommand command)
    {
        return await _service.PublishEventAsync(
            command.EventId,
            command.OrganizerId);
    }

    // US-304 View Events
    public async Task<List<EventListDto>> Handle(GetMyEventsQuery query)
    {
        return await _service.GetMyEventsAsync(
            query.OrganizerId);
    }

    // US-304 Edit Published Event
    public async Task<Result> Handle(UpdatePublishedEventCommand command)
    {
        return await _service.UpdatePublishedEventAsync(
            command.Dto,
            command.OrganizerId);
    }

    // US-304 Edit Draft Event
    public async Task<Result> Handle(UpdateDraftEventCommand command)
    {
        return await _service.UpdateDraftEventAsync(
            command.Dto,
            command.OrganizerId);
    }

    // US-305
    public async Task<Result> Handle(CancelEventCommand command)
    {
        return await _service.CancelEventAsync(
            command.EventId,
            command.OrganizerId);
    }

    // US-306
    public async Task<Result> Handle(BlockSeatsCommand command)
    {
        return await _service.BlockSeatsAsync(
            command.Dto,
            command.OrganizerId);
    }
    public async Task<PagedResult<EventCardDto>> Handle(GetUpcomingEventsQuery query)
    {
        return await _service.GetUpcomingEventsAsync(
            query.Dto);
    }

}