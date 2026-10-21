namespace EventHub.Application.Features.Authentication.Commands;

public record LoginUserCommand(
    string Email,
    string Password);