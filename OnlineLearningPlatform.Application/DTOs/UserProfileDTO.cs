using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.DTOs
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserDTO UserDTO { get; set; }
        public string? Website { get; set; }
        public string? Bio { get; set; }
    }
}
