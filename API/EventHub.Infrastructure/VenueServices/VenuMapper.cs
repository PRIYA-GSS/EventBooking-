using AutoMapper;
using dto= EventHub.Application.Features.VenueFeature.DTOs;
using entity= EventHub.Domain.Entities;

namespace EventHub.Infrastructure.VenueServices;
public class VenueMapping:Profile
{
    public VenueMapping()
    {
        CreateMap<entity.Venue,dto.Venue>().ReverseMap();
    }
}