using EventHub.Application.Features.Authentication.DTOs;
using EventHub.Application.Common.Interfaces;
using EventHub.Application.Features.Authentication.Commands;
using EventHub.Application.Common.Models;
namespace EventHub.Application.Features.Authentication.Handlers;
public class RefreshTokenHandler
{
    private readonly IIdentityService _identityService;

    public RefreshTokenHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<RefreshTokenResponse>> HandleAsync(
        RefreshTokenCommand command)
    {
        var response =
            await _identityService
                  .RefreshTokenAsync(
                      command.RefreshToken);
        return new Result<RefreshTokenResponse>{IsSuccess=true,Message="Refresh token generated",Data=response};

        
    }
}