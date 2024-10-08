using AutoMapper;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region UserProfile
            CreateMap<UserProfile, UserProfileDTO>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap()
                    .ForMember(dest => dest.UserId, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore());
            #endregion

            #region AppUser

            // AppUser -> UserDTO
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ReverseMap()
                    .ForMember(dest => dest.UserProfile, opt => opt.Ignore());

            // AppUser -> UserActivityDTO
            CreateMap<AppUser, UserActivityDTO>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
            #endregion
        }
    }
}
