

using Application.Activities;
using AutoMapper;
using Domain;

namespace Application.Core
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<Activity, Activity>();
      CreateMap<Activity, ActivityDto>()
        .ForMember(
          destination => destination.HostUsername,
          output => output.MapFrom(source => source.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName)
        );
      CreateMap<ActivityAttendee, Profiles.Profile>()
        .ForMember(
          destination => destination.DisplayName,
          output => output.MapFrom(source => source.AppUser.DisplayName)
        )
        .ForMember(
          destination => destination.Username,
          output => output.MapFrom(source => source.AppUser.UserName)
        )
        .ForMember(
          destination => destination.Bio,
          output => output.MapFrom(source => source.AppUser.Bio)
        );
    }
  }
}