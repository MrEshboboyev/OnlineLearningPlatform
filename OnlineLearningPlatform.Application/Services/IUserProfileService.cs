﻿using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<ResponseDTO<UserProfileDTO>> GetProfileAsync(string userId);
        Task<ResponseDTO<bool>> UpdateProfileAsync(UserProfileDTO userProfileDTO);
    }
}
