namespace OnlineLearningPlatform.Application.DTOs;

public class CourseDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Level { get; set; }
    public DateTime CreatedDate { get; set; }

    public string InstructorId { get; set; }
    public string InstructorName { get; set; }

    public ICollection<ModuleDTO> Modules { get; set; }
    public ICollection<EnrollmentDTO> Enrollments { get; set; }
    public ICollection<QuizDTO> Quizzes { get; set; }
}
