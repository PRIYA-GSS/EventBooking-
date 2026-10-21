using AutoMapper;
using EventHub.Application.Features.Profile.DTOs;
using EventHub.Infrastructure.Identity;

public class IdentityMapping:Profile
{
    public IdentityMapping()
    {
        CreateMap<ApplicationUser,UserProfile>()
            .ForMember(dest=>dest.FirstName,opt=>opt.MapFrom(src=>src.FullName.Split(' ',StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()??string.Empty))
            .ForMember(dest=>dest.LastName,opt=>opt.MapFrom(src=>src.FullName.Split(' ',StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault()??string.Empty));
        CreateMap<UserProfile,ApplicationUser>()
            .ForMember(dest=>dest.FullName,opt=>opt.MapFrom(src=>src.FirstName+src.LastName));
    }
}