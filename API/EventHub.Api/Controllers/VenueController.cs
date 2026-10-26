using EventHub.Application.Features.VenueFeature.Commands;
using EventHub.Profiles.Features.VenueFeature.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Api.Controllers;

[ApiController]
[Route("api/venue")]

public class VenueController : ControllerBase
{
    private readonly VenueHandler _handler;
    public VenueController(VenueHandler handler)
    {
        _handler=handler;
    }
    [HttpPost]
    public async Task<IActionResult> AddVenue(CreateVenueCommand command)
    {
        var result= await _handler.AddHandler(command);
        if(!result.IsSuccess)
        return BadRequest(result);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> updateVenue(UpdateVenueCommand command)
    {
        var result= await _handler.updateHandle(command);
        if(!result.IsSuccess)
        return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVenues(GetAllVenuesQuery command)
    {
        var result= await _handler.GetAllHandle(command);
        if(result is null)
        return BadRequest("Could not find venues");

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(GetVenueByIdQuery command)
    {
        var result= await _handler.GetHandle(command);
        if(result is null)
        return BadRequest("Could not find venues");

        return Ok(result);
    }
    [HttpPost]

    public async Task<IActionResult> CreateVenueLayout(CreateLayout command)
    {
        var result= await _handler.CreateLayoutAsync(command);
        if(!result.IsSuccess)
        return BadRequest(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ActivateVenueToggle(ActivateCommand command)
    {
        var result= await _handler.ActivateOrDeactivate(command);
        if(!result.IsSuccess)
        return BadRequest(result);

        return Ok(result);
    }
}