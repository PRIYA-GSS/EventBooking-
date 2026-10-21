using EventHub.Application.Features.VenueFeature.Interface;
using EventHub.Infrastructure.Persistance;
using entity = EventHub.Domain.Entities;
using dto = EventHub.Application.Features.VenueFeature.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EventHub.Application.Common.Models;
using EventHub.Domain.Enums;
namespace EventHub.Infrastructure.VenueServices;

public class VenueService : IVenueInterface
{
    private readonly EventHubDbContext _context;
    private readonly IMapper _mapper;
    public VenueService(
        EventHubDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<bool> ExistsAsync(
        string name,
        string city)
    {
        var venue = await _context.Venues.FirstOrDefaultAsync(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase) && string.Equals(x.City, city, StringComparison.OrdinalIgnoreCase));
        return venue is not null;
    }
    public async Task AddAsync(dto.Venue venue)
    {
        var venueEntity = new entity.Venue();
        _mapper.Map(venue, venueEntity);
        await _context.Venues.AddAsync(venueEntity);
        await _context.SaveChangesAsync();

    }
    public async Task<dto.Venue> GetByIdAsync(Guid id)
    {
        var venue = await _context.Venues.Include(x => x.Sections).ThenInclude(x => x.Seats).FirstOrDefaultAsync(x => x.Id == id);
        var venueDto = _mapper.Map<dto.Venue>(venue);
        return venueDto;

    }
    public async Task UpdateAsync(dto.Venue venue)
    {
        var entity = await _context.Venues.Include(x => x.Sections).ThenInclude(x => x.Seats).FirstOrDefaultAsync(x => x.Id == venue.Id);
        _mapper.Map(entity, venue);
        _context.Venues.Update(entity);
        await _context.SaveChangesAsync();

    }
    public async Task<List<dto.Venue>> GetVenuesAsync( string? search,
    string? city,
    int page,
    int pageSize)
    {
        var query = _context.Venues.Include(x => x.Sections).ThenInclude(x => x.Seats).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Name.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(x =>
                x.City.Contains(city));
        }

        var venues = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var venueList = _mapper.Map<List<dto.Venue>>(venues);
        return venueList;

    }
    public async Task<bool> HasPublishedEventsAsync(Guid venueId)
    {
        return await _context.Events
        .AnyAsync(e =>
            e.VenueId == venueId &&
            e.Status == EventStatus.Published);
    }
    public async Task AddSectionsAsync(List<entity.VenueSection> sections)
    {
        await _context.VenueSections.AddRangeAsync(sections);
        await _context.SaveChangesAsync();
    }
    public async Task<Result> DeactivateOrActivateVenueAsync(Guid venueId, bool IsActive)
    {
        var venue = await _context.Venues.FirstOrDefaultAsync(x => x.Id == venueId);

        if (venue == null)
        {
            return new Result { IsSuccess = false, Message = "Venue Not found" };
        }

        venue.IsActive = IsActive;

        await _context.SaveChangesAsync();
        string message = IsActive ? "Venue Activated" : "VenueDeactivated";
        return new Result { IsSuccess = false, Message = message };
    }


}