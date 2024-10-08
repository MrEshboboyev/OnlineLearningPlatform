namespace OnlineLearningPlatform.Application.DTOs;

public class EnrollmentDTO
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public CourseDTO CourseDTO { get; set; }
    public string StudentId { get; set; }
    public UserDTO StudentDTO { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public double ProgressPercentage { get; set; }
}
