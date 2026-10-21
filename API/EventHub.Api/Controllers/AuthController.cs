using EventHub.Application.Common.Models;
using EventHub.Application.Features.Authentication.Commands;
using EventHub.Application.Features.Authentication.DTOs;
using EventHub.Application.Features.Authentication.Handlers;
using EventHub.Application.Features.Profile.Commands;
using EventHub.Application.Features.Profile.DTOs;
using EventHub.Profile.Features.Authentication.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Api.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly RegisterUserHandler _handler;
    private readonly LoginUserHandler _loginhandler;

    private readonly UserProfileHandler _profilehandler;
    private readonly RefreshTokenHandler _refresh;
    private readonly ResetPasswordHandler _reset;
    private readonly ForgotPasswordHandler _forgot;

    public AuthController(RegisterUserHandler handler, LoginUserHandler loginhandler, RefreshTokenHandler refresh, UserProfileHandler profilehandler, ForgotPasswordHandler forgot, ResetPasswordHandler reset)
    {
        _loginhandler = loginhandler;
        _handler = handler;
        _refresh = refresh;
        _profilehandler = profilehandler;
        _forgot = forgot;
        _reset = reset;

    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password);
        var response = await _handler.HandleAsync(command);
        return Ok(response);

    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);

        var result = await _loginhandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(string refreshtoken)
    {
        var command = new RefreshTokenCommand(
           refreshtoken);

        var result = await _refresh.HandleAsync(command);

        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(string UserId)
    {
        var result = await _loginhandler.LogoutHandleAsync(UserId);
        return Ok(result);
    }
    [HttpGet("view-profile")]
    public async Task<IActionResult> ViewProfile(ViewProfileCommand command)
    {
        var result = await _profilehandler.ViewProfile(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
    ForgotPasswordCommand command)
    {
        var result = await _forgot.Handle(command);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        ResetPasswordCommand command)
    {
        var result = await _reset.Handle(command);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}