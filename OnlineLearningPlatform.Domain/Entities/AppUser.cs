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

        // Derived property to check suspension status
        public bool IsSuspended
        {
            get
            {
                return !string.IsNullOrEmpty(SuspensionReason) &&
                       (!SuspensionEndDate.HasValue || SuspensionEndDate > DateTime.Now);
            }
        }

        // One-to-one relationship with UserProfile
        public UserProfile UserProfile { get; set; }

        // Relationships with other entities
        public ICollection<Course> CoursesTaught { get; set; }  // Instructor for Courses
        public ICollection<Enrollment> Enrollments { get; set; } // Enrolled as Student in Courses
        public ICollection<QuizSubmission> QuizSubmissions { get; set; } // Quiz Submissions
        public ICollection<QuizAttempt> QuizAttempts { get; set; } // Quiz Attempts
    }
}
