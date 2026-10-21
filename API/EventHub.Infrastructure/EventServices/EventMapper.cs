using dto=EventHub.Application.Features.EventFeature.DTOs;
using entity=EventHub.Domain.Entities;
using AutoMapper;

namespace EventHub.Infrastructure.EventMapper;

public class EventMapping : Profile
{
    public EventMapping()
    {
        CreateMap<dto.CreateEventDto, entity.Event>();

        CreateMap<dto.UpdateDraftEvent,entity.Event>();

        CreateMap<entity.Event, dto.EventListDto>();

        CreateMap<entity.Event, dto.EventDetailsDto>();

        CreateMap<entity.EventPrice, dto.CategoryPriceDto>();
    }
}