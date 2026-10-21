using EventHub.Application.Common.Models;
using EventHub.Application.Features.Authentication.DTOs;
using EventHub.Application.Features.Profile.DTOs;
namespace EventHub.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> EmailExistsAsync(string email);

    Task<Result> CreateUserAsync(
        string firstName,
        string lastName,
        string email,
        string password);

    Task<Result> AddToRoleAsync(string email, string role);

    Task<string> GetUserIdAsync(string email);
    Task<Result> LoginAsync(
        string email,
        string password);

    Task<IList<string>> GetRolesAsync(
        string email);
    Task<RefreshTokenResponse> RefreshTokenAsync(
    string refreshToken);

    Task AddRefreshToken(string refreshtoken,string UserId);
    Task<Result> LogoutAsync(string userId);
     Task<UserProfile> ViewProfile(string UserId);
     Task<Result> UpdateProfile(UserProfile profile,string userId);
     Task ForgotPasswordAsync(string email);

    Task<Result> ResetPasswordAsync(
        string email,
        string token,
        string newPassword);
}