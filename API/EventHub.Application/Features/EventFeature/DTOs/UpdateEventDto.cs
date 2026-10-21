using EventHub.Domain.Enums;
namespace EventHub.Application.Features.EventFeature.DTOs;
public class UpdateEventDto
{
    public Guid EventId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
}