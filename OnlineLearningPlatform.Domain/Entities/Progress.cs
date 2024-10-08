namespace OnlineLearningPlatform.Domain.Entities;

public class Progress
{
    public int Id { get; set; }
    public string StudentId { get; set; }
    public AppUser Student { get; set; }
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CompletionDate { get; set; }
}
