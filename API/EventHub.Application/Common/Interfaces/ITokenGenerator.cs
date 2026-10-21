namespace EventHub.Application.Common.Interfaces;

public interface ITokenGenerator
{
    string GenerateToken(
        string userId,
        string email,
        IList<string> roles);

    string RefreshToken();
}