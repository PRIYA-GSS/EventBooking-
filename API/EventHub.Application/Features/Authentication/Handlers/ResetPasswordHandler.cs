using EventHub.Application.Common.Models;
using EventHub.Application.Common.Interfaces;
public class ResetPasswordHandler
{
    private readonly IIdentityService _identityService;

    public ResetPasswordHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(
        ResetPasswordCommand request)
    {
        return await _identityService.ResetPasswordAsync(
            request.Email,
            request.Token,
            request.NewPassword);
    }
}