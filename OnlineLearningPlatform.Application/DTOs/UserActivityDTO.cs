namespace OnlineLearningPlatform.Application.DTOs;

public class UserActivityDTO
{
    public string Id { get; set; }                   // User ID
    public string UserName { get; set; }                 // Username
    public string Email { get; set; }                    // Email address
    public IList<string> Roles { get; set; }                     // User role (e.g., JobSeeker, Employer, Admin)

    public DateTime? LastLoginDate { get; set; }         // Last login date (nullable)

    public bool IsActive { get; set; }                   // Is the user active or deactivated
}
