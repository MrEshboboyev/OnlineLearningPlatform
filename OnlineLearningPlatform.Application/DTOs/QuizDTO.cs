namespace OnlineLearningPlatform.Application.DTOs;

public class QuizDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int CourseId { get; set; }
    public CourseDTO CourseDTO { get; set; }

    public ICollection<QuestionDTO> QuestionDTOs { get; set; }
    public ICollection<QuizSubmissionDTO> QuizSubmissionDTOs { get; set; }
}
