using AutoMapper;
using EventHub.Application.Features.Profile.DTOs;
using dto= EventHub.Application.Features.VenueFeature.DTOs;
using entity= EventHub.Domain.Entities;
using EventHub.Infrastructure.Identity;

public class VenueMapping:Profile
{
    public VenueMapping()
    {
        CreateMap<entity.Venue,dto.Venue>().ReverseMap();
    }
}