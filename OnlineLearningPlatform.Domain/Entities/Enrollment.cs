namespace OnlineLearningPlatform.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public string StudentId { get; set; }
    public AppUser Student { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public double ProgressPercentage { get; set; }
}
