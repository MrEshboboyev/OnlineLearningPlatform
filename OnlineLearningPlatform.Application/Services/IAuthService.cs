using OnlineLearningPlatform.Application.Common.Models;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDTO<string>> LoginAsync(LoginModel loginModel);
        Task<ResponseDTO<string>> RegisterAsync(RegisterModel registerModel);
        Task<ResponseDTO<string>> GenerateJwtToken(AppUser user, IEnumerable<string> roles);

        Task<ResponseDTO<bool>> EmailExist(string email);
        Task<ResponseDTO<bool>> UserNameExist(string username);
    }
}
