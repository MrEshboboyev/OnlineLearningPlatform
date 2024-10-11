namespace OnlineLearningPlatform.Application.DTOs;

public class ProgressDTO
{
    public int Id { get; set; }
    public string StudentId { get; set; }
    public UserDTO StudentDTO { get; set; }
    public int LessonId { get; set; }
    public LessonDTO LessonDTO { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CompletionDate { get; set; }
}
