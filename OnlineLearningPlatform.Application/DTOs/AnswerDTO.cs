namespace OnlineLearningPlatform.Application.DTOs;

public class AnswerDTO
{
    public int Id { get; set; }
    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public QuestionDTO QuestionDTO { get; set; }
}
