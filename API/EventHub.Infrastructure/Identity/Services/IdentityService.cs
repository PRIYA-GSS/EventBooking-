using EventHub.Application.Common.Interfaces;
using EventHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using EventHub.Application.Common.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using EventHub.Application.Features.Authentication.DTOs;
using EventHub.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using EventHub.Infrastructure.Identity.Jwt;
using EventHub.Domain.Entities;
using EventHub.Application.Features.Profile.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace EventHub.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly EventHubDbContext _context;
    private readonly TokenGenerator _token;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;
    public IdentityService(UserManager<ApplicationUser> userManager, UserManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, EventHubDbContext context, TokenGenerator token, IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
        _token = token;
        _emailService = emailService;
    }
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }

    public async Task<Result> AddToRoleAsync(string email, string role)
    {
        var result = new Result();
        var roleExists = await _roleManager.FindByNameAsync(role);
        if (roleExists is null)
        {
            var addRole = await _roleManager.CreateAsync(new IdentityRole(role));
            if (!addRole.Succeeded)
            {
                result.Message = "Role could not be created";
                return result;
            }
        }
        var user = await _userManager.FindByEmailAsync(email);
        var addToRole = await _userManager.AddToRoleAsync(user, role);
        if (!addToRole.Succeeded)
        {
            result.Message = "Role could not be added";
            return result;
        }
        result.IsSuccess = true;
        result.Message = "Role added to user successfully";
        return result;
    }
    public async Task<string> GetUserIdAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return string.Empty;

        return user.Id;
    }
    public async Task<Result> CreateUserAsync(
       string firstName,
       string lastName,
       string email,
       string password)
    {
        var result = new Result();
        var user = new ApplicationUser
        {
            FullName = firstName + lastName,
            Email = email,
            UserName = email
        };
        var createUser = await _userManager.CreateAsync(user);
        if (!createUser.Succeeded)
        {
            result.Message = "Trouble creating user";
            return result;
        }
        result.IsSuccess = true;
        result.Message = "User created successfully";
        return result;
    }
    public async Task<Result> LoginAsync(string email, string password)
    {
        var result = new Result();
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            result.Message = "User does not exist";
            return result;
        }
        var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (checkPassword.IsLockedOut)
        {
            result.Message = "Your account is locked";
            return result;

        }
        if (!checkPassword.Succeeded)
        {
            result.Message = "Invalid email or password";
            return result;
        }
        result.IsSuccess = true;
        result.Message = "Login success";
        return result;
    }
    public async Task<IList<string>> GetRolesAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var roles = await _userManager.GetRolesAsync(user!);
        return roles;
    }
    public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshtoken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshtoken);

        var user = await _userManager.FindByIdAsync(token.UserId);
        var roles = await _userManager.GetRolesAsync(user);
        if (token is null)
            return null;

        if (token.ExpiresAt < DateTime.UtcNow)
            return null;
        if (token.IsRevoked)
        {
            var userTokens =
                await _context.RefreshTokens
                    .Where(x => x.UserId == token.UserId)
                    .ToListAsync();

            foreach (var item in userTokens)
            {
                item.IsRevoked = true;
                item.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return null;
        }
        var newAccessToken = _token.GenerateToken(token.UserId, user.Email, roles);
        var newRefreshToken = _token.RefreshToken();

        token.IsRevoked = true;
        token.RevokedAt = DateTime.UtcNow;
        token.ReplacedByToken = newRefreshToken;

        _context.RefreshTokens.Add(new RefreshToken { Token = newRefreshToken, UserId = token.UserId, CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow });
        await _context.SaveChangesAsync();

        return new RefreshTokenResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken };


    }
    public async Task AddRefreshToken(string refreshToken, string UserId)
    {
        _context.RefreshTokens.Add(new RefreshToken { Token = refreshToken, UserId = UserId, CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow });
        await _context.SaveChangesAsync();

    }
    public async Task<Result> LogoutAsync(string userId)
    {
        var refreshTokens = await _context.RefreshTokens.Where(x => x.UserId == userId && !x.IsRevoked).ToListAsync();
        foreach (var token in refreshTokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();
        return new Result
        {
            IsSuccess = true,
            Message = "Logged out successfully"
        };
    }
    public async Task<UserProfile> ViewProfile(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var userprofile = _mapper.Map<UserProfile>(user);

        return userprofile;
    }
    public async Task<Result> UpdateProfile(UserProfile profile, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new Result { IsSuccess = false, Message = "User not found" };
        _mapper.Map(profile, user);
        var result = await _userManager.UpdateAsync(user);
        return new Result { IsSuccess = true, Message = "Profile updated" };

    }
    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var link = $"{_config["FrontendUrl"]}/reset-password" + $"?email={email}" + $"&token={encodedToken}";
        var htmlBody = BuildResetPasswordEmail(link);

        await _emailService.SendEmailAsync(email, "ResetPassword", htmlBody);
    }
    public async Task<Result> ResetPasswordAsync(string email, string token, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new Result
            {
                IsSuccess = false,
                Message = "Invalid request"
            };
        }
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, password);
        if (!result.Succeeded)
        {
            return new Result
            {
                IsSuccess = false,
                Message = string.Join(",",
                    result.Errors.Select(x => x.Description))
            };
        }
        return new Result
        {
            IsSuccess = true,
            Message = "Password reset successful"
        };


    }
    private string BuildResetPasswordEmail(string resetLink)
    {
        return $@"
        <h2>Reset Password</h2>
        <p>Click the link below to reset your password:</p>

        <p>
            {resetLink}
                Reset Password
            </a>
        </p>

        <p>This link expires in 5 minutes.</p>";
    }

}