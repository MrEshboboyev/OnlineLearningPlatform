namespace OnlineLearningPlatform.Application.DTOs;

public class LessonDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Order { get; set; }
    public int ModuleId { get; set; }
    public ModuleDTO ModuleDTO { get; set; }
}
