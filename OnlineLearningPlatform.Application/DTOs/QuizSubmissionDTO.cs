namespace OnlineLearningPlatform.Application.DTOs;

public class QuizSubmissionDTO
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public QuizDTO QuizDTO { get; set; }
    public string StudentId { get; set; }
    public UserDTO StudentDTO { get; set; }
    public DateTime SubmissionDate { get; set; }
    public double Grade { get; set; }
}
