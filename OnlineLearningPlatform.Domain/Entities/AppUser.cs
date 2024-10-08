using Microsoft.AspNetCore.Identity;

namespace OnlineLearningPlatform.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public DateTime DateRegistered { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;

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

        public UserProfile UserProfile { get; set; }
    }
}
