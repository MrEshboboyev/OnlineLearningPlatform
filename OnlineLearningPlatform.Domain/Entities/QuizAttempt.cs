namespace OnlineLearningPlatform.Domain.Entities;

public class QuizAttempt
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string UserId { get; set; } // The user who took the quiz
    public DateTime AttemptDate { get; set; } // Date of attempt
    public int Score { get; set; } // Score achieved

    // Track the time user took for this attempt (in minutes)
    public int TimeTakenInMinutes { get; set; }

    // Whether this attempt was within allowed time or not
    public bool IsTimedOut { get; set; }

    public Quiz Quiz { get; set; }
    public AppUser User { get; set; }
}