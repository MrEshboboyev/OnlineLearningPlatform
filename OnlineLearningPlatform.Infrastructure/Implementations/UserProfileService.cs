using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services.Interfaces;

namespace OnlineLearningPlatform.Infrastructure.Implementations
{
    public class UserProfileService(IUnitOfWork unitOfWork,
        IMapper mapper) : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDTO<UserProfileDTO>> GetProfileAsync(string userId)
        {
            try
            {
                var profile = await _unitOfWork.UserProfile.GetAsync(
                    filter: userProfile => userProfile.UserId.Equals(userId),
                    includeProperties: "User"
                    ) ?? throw new Exception("Profile not found!");

                var mappedProfile = _mapper.Map<UserProfileDTO>(profile);

                return new ResponseDTO<UserProfileDTO>(mappedProfile);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserProfileDTO>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> UpdateProfileAsync(UserProfileDTO userProfileDTO)
        {
            try
            {
                var profile = await _unitOfWork.UserProfile.GetAsync(
                    filter: userProfile => userProfile.UserId.Equals(userProfileDTO.UserId) &&
                    userProfile.Id.Equals(userProfileDTO.Id)
                    ) ?? throw new Exception("Profile not found!");

                // update fields
                _mapper.Map(userProfileDTO, profile);

                // update
                await _unitOfWork.UserProfile.UpdateAsync(profile);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }
    }
}
