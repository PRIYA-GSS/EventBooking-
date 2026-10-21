using EventHub.Domain.Enums;
namespace EventHub.Application.Features.EventFeature.DTOs;
public class EventSearchDto
{
    public string? Search { get; set; }

    public string? City { get; set; }

    public EventCategory? Category { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 12;
}