namespace OnlineLearningPlatform.Application.DTOs;

public class QuizDTO
{
    public int Id { get; set; }
    public string Title { get; set; }

    // Maximum time allowed for the quiz (in minutes)
    public int? TimeLimitInMinutes { get; set; }

    // Number of allowed retakes (null or 0 for unlimited)
    public int AllowedRetakes { get; set; }
    public int CourseId { get; set; }
    public CourseDTO CourseDTO { get; set; }

    public ICollection<QuestionDTO> QuestionDTOs { get; set; }
    public ICollection<QuizSubmissionDTO> QuizSubmissionDTOs { get; set; }
}
