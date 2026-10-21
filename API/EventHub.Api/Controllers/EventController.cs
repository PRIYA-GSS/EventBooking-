
using EventHub.Application.Features.EventFeature.Commands;
using EventHub.Application.Features.EventFeature.Handler;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Api.Controllers;

[ApiController]
[Route("api/event")]

public class EventController : ControllerBase
{
    private readonly EventsHandler _handler;
    public EventController(EventsHandler handler)
    {
        _handler = handler;
    }
    [HttpPost]
    public async Task<IActionResult> CreateEvent(CreateEventCommand command)
    {
        var result = await _handler.Handle(command);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpPost("prices")]
    public async Task<IActionResult> SetPrices(
        SetPricesCommand command)
    {
        var result = await _handler.Handle(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // US-303 Publish Event
    [HttpPatch("publish")]
    public async Task<IActionResult> PublishEvent(
        PublishEventCommand command)
    {
        var result = await _handler.Handle(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // US-304 View My Events
    [HttpGet("my-events")]
    public async Task<IActionResult> GetMyEvents(
        [FromQuery] GetMyEventsQuery query)
    {
        var result = await _handler.Handle(query);

        return Ok(result);
    }

    // US-304 Update Draft Event
    [HttpPut("draft")]
    public async Task<IActionResult> UpdateDraftEvent(
        UpdateDraftEventCommand command)
    {
        var result = await _handler.Handle(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // US-304 Update Published Event
    [HttpPut("published")]
    public async Task<IActionResult> UpdatePublishedEvent(
        UpdatePublishedEventCommand command)
    {
        var result = await _handler.Handle(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // US-305 Cancel Event
    [HttpPatch("cancel")]
    public async Task<IActionResult> CancelEvent(
        CancelEventCommand command)
    {
        var result = await _handler.Handle(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // US-306 Block Seats
    [HttpPatch("block-seats")]
    public async Task<IActionResult> BlockSeats(
        BlockSeatsCommand command)
    {
        var result = await _handler.Handle(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetEvents(GetUpcomingEventsQuery query)
    {
        var result = await _handler.Handle(query);

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventDetails(
    GetEventDetailsQuery query)
    {
        var result =
            await _handler.Handle(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }


}