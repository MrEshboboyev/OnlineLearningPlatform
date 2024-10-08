namespace OnlineLearningPlatform.Application.DTOs;

public class ModuleDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int CourseId { get; set; }
    public CourseDTO CourseDTO { get; set; }

    public ICollection<LessonDTO> LessonDTOs { get; set; }
}
