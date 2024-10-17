namespace OnlineLearningPlatform.Application.DTOs;

public class StudentProgressDTO
{
    public string StudentId { get; set; }
    public string UserName { get; set; }
    public int TotalLessons { get; set; }
    public int CompletedLessons { get; set; }
    public int CourseId { get; set; }
}
