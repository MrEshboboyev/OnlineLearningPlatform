namespace OnlineLearningPlatform.Domain.Entities;

public class Module
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }

    // The order of the module within the course
    public int Order { get; set; }

    public ICollection<Lesson> Lessons { get; set; }
}