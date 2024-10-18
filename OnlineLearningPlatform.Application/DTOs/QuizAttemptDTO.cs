namespace OnlineLearningPlatform.Application.DTOs;

public class QuizAttemptDTO
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string UserId { get; set; }
    public DateTime AttemptDate { get; set; } 
    public int Score { get; set; } 

    public int TimeTakenInMinutes { get; set; }

    public bool IsTimedOut { get; set; }

    public QuizDTO QuizDTO { get; set; }
    public UserDTO UserDTO { get; set; }
}
