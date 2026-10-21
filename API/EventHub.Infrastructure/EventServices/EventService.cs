using EventHub.Application.Features.VenueFeature.Interface;
using EventHub.Infrastructure.Persistance;
using entity = EventHub.Domain.Entities;
using dto = EventHub.Application.Features.EventFeature.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EventHub.Application.Common.Models;
using EventHub.Domain.Enums;
using EventHub.Application.Features.EventFeature.Interface;
using EventHub.Domain.Entities;
using EventHub.Application.Features.VenueFeature.Commands;
using EventHub.Application.Features.EventFeature.DTOs;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
namespace EventHub.Infrastructure.EventServices;

public class EventService : IEventService
{
    private readonly EventHubDbContext _context;
    private readonly IMapper _mapper;
    public EventService(
        EventHubDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> CreateEventAsync(
    dto.CreateEventDto dto)
    {
        var result = new Result();
        var venueExists = await _context.Venues
            .AnyAsync(x => x.Id == dto.VenueId);

        if (!venueExists)
        {
            result.IsSuccess = false;
            result.Message = "Venue not found";
            return result;
        }
        if (dto.StartTime <= DateTime.UtcNow)
        {
            result.IsSuccess = false;
            result.Message = "Start time must be in the future";
            return result;
        }
        if (dto.EndTime <= dto.StartTime)
        {
            result.IsSuccess = false;
            result.Message = "End time must be after start time";
            return result;
        }
        var overlap = await _context.Events.AnyAsync(x =>
                        x.VenueId == dto.VenueId &&
                        x.Status != EventStatus.Cancelled &&
                        dto.StartTime < x.EndTime &&
                        dto.EndTime > x.StartTime);
        if (overlap)
        {
            result.IsSuccess = false;
            result.Message = "An event is already scheduled at this time in this venue";
            return result;
        }
        var eventEntity = _mapper.Map<Event>(dto);
        eventEntity.Status = EventStatus.Draft;

        await _context.Events.AddAsync(eventEntity);
        await _context.SaveChangesAsync();

        result.IsSuccess = true;
        result.Message = $"event scheduled successfully at {dto.StartTime}";
        return result;
    }
    public async Task<Result> SetPricesAsync(
    SetPricesDto dto,
    string organizerId)
    {
        var eventEntity = await _context.Events.FirstOrDefaultAsync(x => x.Id == dto.EventId);
        if (eventEntity == null)
        {
            return new Result { IsSuccess = false, Message = "Event Not found" };
        }
        if (eventEntity.Status == EventStatus.Published)
            return new Result { IsSuccess = false, Message = "No actions allowed on published events" };

        var venue = await _context.Venues.Include(x => x.Sections).FirstOrDefaultAsync(x => x.Id == eventEntity.VenueId);

        if (venue == null)
        {
            return new Result { IsSuccess = false, Message = "Venue Not found" };
        }
        var categories = venue.Sections.Select(x => x.Category).Distinct().ToList();
        var eventPriceList = new List<EventPrice>();
        var IsallcategoriesPriced = AllCategoriesPriced(categories, dto.Prices);

        if (!IsallcategoriesPriced)
            return new Result { IsSuccess = false, Message = "All Categories need to be priced" };

        var existingPrices = await _context.EventPrice
        .Where(x => x.EventId == dto.EventId)
        .ToListAsync();

        _context.EventPrice.RemoveRange(existingPrices);
        foreach (var price in dto.Prices)
        {
            eventPriceList.Add(new EventPrice
            {
                EventId = dto.EventId,
                Category = price.Category,
                Price = price.Price

            });
        }
        await _context.EventPrice.AddRangeAsync(eventPriceList);
        await _context.SaveChangesAsync();
        return new Result { IsSuccess = true, Message = "category prices for Event are recorded" };
    }
    public async Task<Result> PublishEventAsync(
    Guid eventId,
    string organizerId)
    {
        var eventEntity = await _context.Events.FirstOrDefaultAsync(x => x.Id == eventId && x.OrganizerId == organizerId);
        if (eventEntity == null)
            return new Result { IsSuccess = false, Message = "Event Not found" };

        if (eventEntity.Status == EventStatus.Published)
            return new Result { IsSuccess = false, Message = "Event already published" };
        var venue = await _context.Venues.Include(x => x.Sections).ThenInclude(x => x.Seats).FirstOrDefaultAsync(x => x.Id == eventEntity.VenueId);
        if (venue == null)
        {
            return new Result
            {
                IsSuccess = false,
                Message = "Venue not found"
            };
        }
        var categoryPrices = await _context.EventPrice.Where(x => x.EventId == eventId).Select(x => new CategoryPriceDto { Category = x.Category, Price = x.Price }).ToListAsync();
        var categories = venue.Sections.Select(x => x.Category).ToList();

        var IsallcategoriesPriced = AllCategoriesPriced(categories, categoryPrices);

        if (!IsallcategoriesPriced)
            return new Result { IsSuccess = false, Message = "All Categories need to be priced" };

        using var transaction = await _context.Database.BeginTransactionAsync();
        var eventSeats = venue.Sections.SelectMany(x => x.Seats).Select(x => new EventSeat
        {
            EventId = eventId,
            SeatId = x.Id,
            Status = SeatStatus.Available
        }).ToList();
        await _context.EventSeats.AddRangeAsync(eventSeats);

        eventEntity.Status = EventStatus.Published;
        eventEntity.OrganizerId = organizerId;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return new Result { IsSuccess = true, Message = "Published event with seats availability successfully" };
    }
    public async Task<List<EventListDto>> GetMyEventsAsync(
    string organizerId)
    {
        var events = await _context.Events.Where(x => x.OrganizerId == organizerId).OrderBy(x => x.Status).ToListAsync();
        var eventlist = _mapper.Map<List<EventListDto>>(events);
        return eventlist;
    }
    public async Task<Result> UpdatePublishedEventAsync(UpdateEventDto dto,
    string organizerId)
    {
        var eventEntity = await _context.Events.FirstOrDefaultAsync(x => x.Id == dto.EventId && x.OrganizerId == organizerId);

        if (eventEntity is null)
        {
            return new Result { IsSuccess = false, Message = "Could not find event" };
        }
        if (eventEntity.Status != EventStatus.Published)
        {
            return new Result { IsSuccess = false, Message = "Only published events are updated here" };
        }
        eventEntity.Title = dto.Title;
        eventEntity.Description = dto.Description;
        await _context.SaveChangesAsync();
        return new Result { IsSuccess = true, Message = "Event Updated" };
    }
    public async Task<Result> UpdateDraftEventAsync(UpdateDraftEvent dto,
    string organizerId)
    {
        var result = new Result();
        if (dto.StartTime <= DateTime.UtcNow || dto.EndTime < dto.StartTime)
        {
            result.IsSuccess = false;
            result.Message = "Invalid start and end time";
            return result;
        }
        var overlap = await _context.Events.AnyAsync(x => x.Id != dto.EventId &&
                        x.VenueId == dto.VenueId &&
                        x.Status != EventStatus.Cancelled &&
                        dto.StartTime < x.EndTime &&
                        dto.EndTime > x.StartTime);
        if (overlap)
        {
            result.IsSuccess = false;
            result.Message = "An event is already scheduled at this time in this venue";
            return result;
        }
        var eventEntity = await _context.Events.FirstOrDefaultAsync(x => x.Id == dto.EventId && x.OrganizerId == organizerId);
        if (eventEntity == null)
        {
            result.IsSuccess = false;
            result.Message = "Event not found";
            return result;
        }
        if (eventEntity.Status == EventStatus.Published)
        {
            result.IsSuccess = false;
            result.Message = "This event is already published.";
            return result;
        }
        _mapper.Map(dto, eventEntity);
        await _context.SaveChangesAsync();
        result.IsSuccess = true;
        result.Message = "An event is updated successfully";
        return result;
    }

    public async Task<Result> CancelEventAsync(
    Guid eventId,
    string organizerId)
    {
        var eventEntity = await _context.Events.FirstOrDefaultAsync(x => x.Id == eventId && x.OrganizerId == organizerId);
        if (eventEntity == null)
        {
            return new Result
            {
                IsSuccess = false,
                Message = "Event not found"
            };
        }
        if (eventEntity.StartTime <= DateTime.UtcNow)
        {
            return new Result { IsSuccess = false, Message = "Already started cannot be cancel" };
        }
        eventEntity.Status = EventStatus.Cancelled;
        await _context.SaveChangesAsync();
        return new Result { IsSuccess = true, Message = "Event cancelled" };
    }
    public async Task<Result> BlockSeatsAsync(
    BlockSeatsDto dto,
    string organizerId)
    {
        var eventEntity = await _context.Events
         .FirstOrDefaultAsync(x =>
        x.Id == dto.EventId &&
        x.OrganizerId == organizerId);

        if (eventEntity == null)
        {
            return new Result { IsSuccess = false, Message = "Event not found" };
        }
        var seats = await _context.EventSeats.Where(x => x.EventId == dto.EventId && dto.EventSeatIds.Contains(x.Id)).ToListAsync();
        if (seats.Any(x => x.Status != SeatStatus.Available))
        {
            return new Result { IsSuccess = false, Message = "Only available seats can be blocked" };
        }
        foreach (var seat in seats)
        {
            seat.Status = SeatStatus.Blocked;
        }
        await _context.SaveChangesAsync();
        return new Result { IsSuccess = true, Message = "Seats blocked successfully" };
    }

    public async Task<PagedResult<EventCardDto>> GetUpcomingEventsAsync(
    EventSearchDto dto)
    {
        var query = _context.Events
            .Where(x =>
                x.Status == EventStatus.Published &&
                x.StartTime > DateTime.UtcNow)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(dto.Search))
        {
            var search = dto.Search.ToLower();

            query = query.Where(x =>
                x.Title.ToLower().Contains(search) ||
                x.Description.ToLower().Contains(search));
        }
        if (!string.IsNullOrWhiteSpace(dto.City))
        {
            query = query.Where(x =>
                x.Venue.City == dto.City);
        }
        if (dto.Category.HasValue)
        {
            query = query.Where(x =>
                x.Category == dto.Category.Value);
        }
        if (dto.FromDate.HasValue)
        {
            query = query.Where(x =>
                x.StartTime >= dto.FromDate.Value);
        }

        if (dto.ToDate.HasValue)
        {
            query = query.Where(x =>
                x.StartTime <= dto.ToDate.Value);
        }
        var totalRecords = await query.CountAsync();
        var items = await query.OrderBy(x => x.StartTime).Skip((dto.Page - 1) * dto.PageSize).Take(dto.PageSize)
                    .Select(x => new EventCardDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        StartTime = x.StartTime,
                        VenueName = x.Venue.Name,
                        City = x.Venue.City,
                        StartingPrice = _context.EventPrice.Where(p => p.EventId == x.Id).Select(p => (decimal?)p.Price).Min() ?? 0,
                        IsSoldOut = !_context.EventSeats.Any(s => s.EventId == x.Id && s.Status == SeatStatus.Available)
                    }).ToListAsync();

        return new PagedResult<EventCardDto>
        {
            Items = items,
            TotalRecords = totalRecords,
            Page = dto.Page,
            PageSize = dto.PageSize
        };
    }
    public async Task<EventDetailsDto?> GetEventDetailsAsync(
    Guid eventId)
    {
        var eventEntity = await _context.Events
            .Include(x => x.Venue)
            .FirstOrDefaultAsync(x =>
                x.Id == eventId &&
                x.Status == EventStatus.Published);

        if (eventEntity == null)
        {
            return null;
        }
        var prices = await _context.EventPrice.Where(x => x.EventId == eventEntity.Id).Select(x => new CategoryPriceDto { Category = x.Category, Price = x.Price }).ToListAsync();

        var seatsRemaining = await _context.EventSeats
                                        .Where(x => x.EventId == eventEntity.Id && x.Status == SeatStatus.Available)
                                        .GroupBy(x => x.Seat.Section.Category)
                                        .Select(x => new CategorySeatAvailabilityDto { Category = x.Key, RemainingSeats = x.Count() })
                                        .ToListAsync();
        return new EventDetailsDto
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Description = eventEntity.Description,

            VenueName = eventEntity.Venue.Name,
            City = eventEntity.Venue.City,

            StartTime = eventEntity.StartTime,
            EndTime = eventEntity.EndTime,

            Prices = prices,
            SeatsRemaining = seatsRemaining
        };
    }
    private bool AllCategoriesPriced(List<SeatCategory> categories, List<CategoryPriceDto> prices)
    {

        return categories.All(category =>
            prices.Any(price =>
                price.Category == category &&
                price.Price > 0));
    }
}
