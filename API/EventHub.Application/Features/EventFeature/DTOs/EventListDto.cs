using EventHub.Domain.Enums;
namespace EventHub.Application.Features.EventFeature.DTOs;
public class EventListDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public EventStatus Status { get; set; }
}