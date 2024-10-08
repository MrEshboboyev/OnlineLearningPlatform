namespace OnlineLearningPlatform.Domain.Entities;

public class Question
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }  // E.g., MultipleChoice, TrueFalse
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }

    public ICollection<Answer> Answers { get; set; }
}