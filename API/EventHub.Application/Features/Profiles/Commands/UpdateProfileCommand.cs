using EventHub.Application.Features.Profiles.DTOs;

namespace EventHub.Application.Features.Profiles.Commands;
public record UpdateProfileCommand(UserProfile profile,string userId);