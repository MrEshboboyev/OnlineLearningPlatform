namespace OnlineLearningPlatform.Domain.Entities
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } // Foreign key to User
        public string? Website { get; set; } 
        public string? Bio { get; set; } // Used for job seekers, or both

        // Navigation property
        public AppUser User { get; set; }
    }
}
