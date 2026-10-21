using EventHub.Application.Features.EventFeature.DTOs;
using EventHub.Application.Common.Models;
namespace EventHub.Application.Features.EventFeature.Interface;

public interface IEventService
{
    Task<Result> CreateEventAsync(
    CreateEventDto dto);
    Task<Result> SetPricesAsync(
    SetPricesDto dto,
    string organizerId);
    Task<Result> PublishEventAsync(
    Guid eventId,
    string organizerId);
    Task<List<EventListDto>> GetMyEventsAsync(
    string organizerId);
    Task<Result> UpdatePublishedEventAsync(UpdateEventDto dto,
    string organizerId);
    Task<Result> UpdateDraftEventAsync(UpdateDraftEvent dto,
    string organizerId);
    Task<Result> CancelEventAsync(
    Guid eventId,
    string organizerId);
    Task<Result> BlockSeatsAsync(
    BlockSeatsDto dto,
    string organizerId);

    Task<PagedResult<EventCardDto>> GetUpcomingEventsAsync(
    EventSearchDto dto);

    Task<EventDetailsDto?> GetEventDetailsAsync(
    Guid eventId);
}