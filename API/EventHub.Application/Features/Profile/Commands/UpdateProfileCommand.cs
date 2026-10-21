using EventHub.Application.Features.Profile.DTOs;

namespace EventHub.Application.Features.Profile.Commands;
public record UpdateProfileCommand(UserProfile profile,string userId);