namespace OnlineLearningPlatform.Domain.Entities;

public class QuizSubmission
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public string StudentId { get; set; }
    public AppUser Student { get; set; }
    public DateTime SubmissionDate { get; set; }
    public double Grade { get; set; }
}
