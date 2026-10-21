using System.Security.Principal;
using EventHub.Application.Common.Models;
using EventHub.Application.Features.Authentication.Commands;
using EventHub.Application.Features.Authentication.DTOs;
using EventHub.Application.Common.Interfaces;

namespace EventHub.Application.Features.Authentication.Handlers;

public class LoginUserHandler
{
    private readonly IIdentityService _identityService;
    private readonly ITokenGenerator _tokenGenerator;
    public LoginUserHandler(IIdentityService identityService, ITokenGenerator tokenGenerator)
    {
        _identityService=identityService;
        _tokenGenerator=tokenGenerator;
    }

    public async Task<Result<LoginResponse>> HandleAsync(LoginUserCommand command)
    {
        var result= new Result<LoginResponse>();
        var loginResult= await _identityService.LoginAsync(command.Email, command.Password);

        if(!loginResult.IsSuccess)
        {
            result.Message= loginResult.Message;
            return result;
        }
        var userId= await _identityService.GetUserIdAsync(command.Email);
        var roles= await _identityService.GetRolesAsync(command.Email);

        var accessToken= _tokenGenerator.GenerateToken(userId,command.Email,roles);

        var refreshToken =_tokenGenerator.RefreshToken();
        await _identityService.AddRefreshToken(refreshToken,userId);

        var response= new LoginResponse{AccessToken=accessToken,RefreshToken=refreshToken,
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15),RefreshTokenExpiry =
                    DateTime.UtcNow.AddDays(7)};

        result.IsSuccess= true;
        result.Message="Login Successful";
        result.Data=response;
        return result;
    }
    public async Task<Result> LogoutHandleAsync(string UserId)
    {
        var result= await _identityService.LogoutAsync(UserId);
        return result;
    }
}