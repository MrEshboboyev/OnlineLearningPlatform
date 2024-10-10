namespace OnlineLearningPlatform.Domain.Entities;

public class Question
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }  // E.g., MultipleChoice, TrueFalse
    public int QuizId { get; set; }

    public int QuestionOrder { get; set; }    // Added property for ordering

    public Quiz Quiz { get; set; }

    public ICollection<Answer> Answers { get; set; }
}