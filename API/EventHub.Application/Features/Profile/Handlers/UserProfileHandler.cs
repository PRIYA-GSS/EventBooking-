
using EventHub.Application.Common.Models;
using EventHub.Application.Features.Profile.Commands;
 using EventHub.Application.Common.Interfaces;
using EventHub.Application.Features.Profile.DTOs;
namespace EventHub.Profile.Features.Authentication.Handlers;

public class UserProfileHandler
{
    private readonly IIdentityService _identityService;
    public UserProfileHandler(IIdentityService identityService)
    {
        _identityService=identityService;
    }
    public async Task<Result<UserProfile>> ViewProfile(ViewProfileCommand command)
    {
        var result= new Result<UserProfile>();
        var response= await _identityService.ViewProfile(command.UserId);
        if(response==null)
        {
            result.Message="User not found";
            return result;
        }

        return new Result<UserProfile>{IsSuccess=true,Message="User details retrieved",Data=response};

    }
    public async Task<Result> UpdateProfile(UpdateProfileCommand command)
    {
        var response= await _identityService.UpdateProfile(command.profile,command.userId);
        return response;
    }
}