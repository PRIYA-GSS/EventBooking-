namespace EventHub.Application.Features.Authentication.DTOs;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime AccessTokenExpiry { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }
}