namespace EventHub.Application.Features.Authentication.Commands;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password);