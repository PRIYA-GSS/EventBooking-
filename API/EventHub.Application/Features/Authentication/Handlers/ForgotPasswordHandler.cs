using EventHub.Application.Common.Interfaces;
using EventHub.Application.Common.Models;
using EventHub.Application.Features.Authentication.Commands;
namespace EventHub.Application.Features.Authentication.Handlers;
public class ForgotPasswordHandler
{
    private readonly IIdentityService _identityService;

    public ForgotPasswordHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(
        ForgotPasswordCommand request)
    {
        await _identityService.ForgotPasswordAsync(
            request.Email);

        return new Result
        {
            IsSuccess = true,
            Message =
                "If an account exists, a password reset link has been sent."
        };
    }
}