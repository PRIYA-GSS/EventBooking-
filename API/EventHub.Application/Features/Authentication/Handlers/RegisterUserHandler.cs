using EventHub.Application.Common.Models;
using EventHub.Application.Features.Authentication.Commands;
using EventHub.Application.Features.Authentication.DTOs;
using EventHub.Application.Common.Interfaces;
namespace EventHub.Application.Features.Authentication.Handlers;

public class RegisterUserHandler
{
    private readonly IIdentityService _identityService;
    private readonly ITokenGenerator _tokenGenerator;
    public RegisterUserHandler(IIdentityService identityService,ITokenGenerator tokenGenerator)
    {
        _identityService=identityService;
        _tokenGenerator=tokenGenerator;
    }

    public async Task<Result<AuthResponse>> HandleAsync(RegisterUserCommand command)
    {
        var result= new Result<AuthResponse>();
        var emailExists= await _identityService.EmailExistsAsync(command.Email);
        if(emailExists)
        {
            result.Message="Email already registered";
            return result;
        }
        var response= await _identityService.CreateUserAsync(command.FirstName,command.LastName,command.Email, command.Password);

        if(!response.IsSuccess)
        {
            result.Message= response.Message;
            return result;
        }
        var Roleresponse= await _identityService.AddToRoleAsync(command.Email,"Customer");
        if(!Roleresponse.IsSuccess)
        {
            result.Message=Roleresponse.Message;
            return result;
        }
        var userId= await _identityService.GetUserIdAsync(command.Email);

        var token= _tokenGenerator.GenerateToken(userId,command.Email,new List<string> {"Customer"});
        var refreshToken= _tokenGenerator.RefreshToken();
        await  _identityService.AddRefreshToken(refreshToken,userId);

        var authResponse= new AuthResponse{UserId=userId,Email=command.Email,Token=token,RefreshToken=refreshToken};
        result.IsSuccess=true;
        result.Message="User Registered!";
        result.Data=authResponse;
        return result;

    }
}