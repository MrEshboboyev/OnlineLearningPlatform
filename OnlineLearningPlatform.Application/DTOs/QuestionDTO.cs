namespace OnlineLearningPlatform.Application.DTOs;

public class QuestionDTO
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public string QuestionType { get; set; } 
    public int QuizId { get; set; }
    public QuizDTO QuizDto { get; set; }

    public ICollection<AnswerDTO> AnswerDTOs { get; set; }
}
