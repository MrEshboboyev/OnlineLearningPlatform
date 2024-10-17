namespace OnlineLearningPlatform.Application.DTOs;

public class StudentCourseProgressDTO
{
    public string CourseTitle { get; set; }
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
}