namespace OnlineLearningPlatform.Application.DTOs;

public class UserDTO
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IList<string> Roles { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateRegistered { get; set; }
    public DateTime? LastLoginDate { get; set; }

    public string? SuspensionReason { get; set; }
    public DateTime? SuspensionEndDate { get; set; }

    // Derived property
    public bool IsSuspended
    {
        get
        {
            return !string.IsNullOrEmpty(SuspensionReason) &&
                   (!SuspensionEndDate.HasValue || SuspensionEndDate > DateTime.Now);
        }
    }

    public DateTimeOffset? LockoutEnd { get; set; }
}
