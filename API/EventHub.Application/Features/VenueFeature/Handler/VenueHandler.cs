using EventHub.Application.Common.Models;
using EventHub.Application.Features.Profile.Commands;
using EventHub.Application.Common.Interfaces;
using EventHub.Application.Features.VenueFeature.DTOs;
using EventHub.Application.Features.VenueFeature.Commands;
using EventHub.Application.Features.VenueFeature.Interface;
using entity = EventHub.Domain.Entities;
namespace EventHub.Profile.Features.VenueFeature.Handlers;

public class VenueHandler
{
    private readonly IVenueInterface _venueService;
    public VenueHandler(IVenueInterface venueService)
    {
        _venueService = venueService;
    }
    public async Task<Result> AddHandler(CreateVenueCommand command)
    {
        var exists = await _venueService.ExistsAsync(command.Name, command.City);
        if (exists)
        {
            return new Result
            {
                IsSuccess = false,
                Message = "Venue already exists in this city."
            };
        }

        var venue = new Venue
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            City = command.City,
            Address = command.Address,
            IsActive = true
        };

        await _venueService.AddAsync(venue);

        return new Result
        {
            IsSuccess = true,
            Message = "Venue created successfully."
        };
    }
    public async Task<Result> updateHandle(
    UpdateVenueCommand command)
    {
        var venue = await _venueService.GetByIdAsync(
            command.Id);

        if (venue is null)
        {
            return new Result
            {
                IsSuccess = false,
                Message = "Venue not found."
            };
        }

        venue.Name = command.Name;
        venue.City = command.City;
        venue.Address = command.Address;

        await _venueService.UpdateAsync(venue);

        return new Result
        {
            IsSuccess = true,
            Message = "Venue updated successfully."
        };
    }
    public async Task<Venue> GetHandle(
    GetVenueByIdQuery query)
    {
        return await _venueService.GetByIdAsync(query.Id);
    }
    public async Task<List<Venue>> GetAllHandle(
           GetAllVenuesQuery query)
    {
        return await _venueService.GetVenuesAsync(query.search,query.city,query.page,query.pageSize);
    }
    public async Task<Result> CreateLayoutAsync(CreateLayout request)
    {
        var result = new Result();
        var venue = await _venueService
          .GetByIdAsync(request.VenueId);

        if (venue == null)
        {
            result.IsSuccess = false;
            result.Message = "Venue Not Found";
            return result;
        }

        var hasPublishedEvents =
            await _venueService.HasPublishedEventsAsync(request.VenueId);

        if (hasPublishedEvents)
        {
            result.IsSuccess = false;
            result.Message = "Event Already published, layout cannot be edited";
            return result;
        }
    
        var sections = new List<entity.VenueSection>();
        foreach (var sectionRequest in request.Sections)
        {
            var section = new entity.VenueSection
            {
                VenueId = request.VenueId,
                Name = sectionRequest.Name,
                Category = sectionRequest.Category
            };
          
            int y = 1;


            foreach (var row in sectionRequest.Rows)
            {
                for (int seatNo = 1;
                     seatNo <= sectionRequest.SeatsPerRow;
                     seatNo++)
                {
                    section.Seats.Add(
                        new entity.Seat
                        {
                            Section = section,
                            Label =
                               $"{sectionRequest.Name[0]}-{row}-{seatNo}",

                            GridX = seatNo,

                            GridY = y
                        });
                }

                y++;
            }
            sections.Add(section);

        }

        await _venueService.AddSectionsAsync(sections);


        result.IsSuccess=true;
        result.Message="layout created successfully";
        return result;
    }
    public async Task<Result> ActivateOrDeactivate(ActivateCommand command)
    {
        var result= await _venueService.DeactivateOrActivateVenueAsync(command.VenueId,command.IsActive);
        return result;

    }
}

