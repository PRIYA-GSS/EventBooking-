using entity = EventHub.Domain.Entities;
using EventHub.Application.Common.Models;
using dto = EventHub.Application.Features.VenueFeature.DTOs;
namespace EventHub.Application.Features.VenueFeature.Interface;

public interface IVenueInterface
{
    Task<bool> ExistsAsync(
        string name,
        string city);

    Task AddAsync(dto.Venue venue);

    Task<dto.Venue?> GetByIdAsync(Guid id);

    Task UpdateAsync(dto.Venue venue);
    Task<List<dto.Venue>> GetVenuesAsync(string? search,
        string? city,
        int page,
        int pageSize);
    Task<bool> HasPublishedEventsAsync(Guid venueId);
    Task AddSectionsAsync(List<entity.VenueSection> sections);
    Task<Result> DeactivateOrActivateVenueAsync(Guid venueId, bool IsActive);

}
