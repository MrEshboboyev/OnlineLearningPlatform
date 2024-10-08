namespace OnlineLearningPlatform.Application.DTOs
{
    public class UserProfileDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string? Website { get; set; }
        public string? Bio { get; set; }
        public string? OwnerName { get; set; }
    }
}
