namespace OnlineLearningPlatform.Domain.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign key to User
        public string? Website { get; set; } 
        public string? Bio { get; set; } 

        // Navigation property
        public AppUser User { get; set; }
    }
}
