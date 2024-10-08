namespace OnlineLearningPlatform.Domain.Entities;

// Course entity
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Level { get; set; }
    public DateTime CreatedDate { get; set; }

    public string InstructorId { get; set; }
    public AppUser Instructor { get; set; }

    public ICollection<Module> Modules { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<Quiz> Quizzes { get; set; }
}