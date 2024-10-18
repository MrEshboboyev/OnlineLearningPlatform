namespace OnlineLearningPlatform.Domain.Entities;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }
    // Maximum time allowed for the quiz (in minutes)
    public int? TimeLimitInMinutes { get; set; }

    // Number of allowed retakes (null or 0 for unlimited)
    public int AllowedRetakes { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }

    public ICollection<Question> Questions { get; set; }
    public ICollection<QuizSubmission> QuizSubmissions { get; set; }
    public ICollection<QuizAttempt> QuizAttempts { get; set; }
}
