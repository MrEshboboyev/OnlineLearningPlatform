using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface IQuizAttemptService
{
    // Gets the quiz attempt by ID
    Task<ResponseDTO<QuizAttemptDTO>> GetQuizAttemptByIdAsync(int attemptId);

    // Gets all attempts for a specific quiz and user
    Task<ResponseDTO<IEnumerable<QuizAttemptDTO>>> GetQuizAttemptsForUserAsync(int quizId, string userId);

    // Checks if the user is allowed to retake the quiz
    Task<ResponseDTO<bool>> CanRetakeQuizAsync(int quizId, string userId);

    // Starts a new quiz attempt for a user
    Task<ResponseDTO<QuizAttemptDTO>> StartQuizAttemptAsync(int quizId, string userId);

    // Submits the quiz attempt and calculates the time taken and score
    Task<ResponseDTO<object>> SubmitQuizAttemptAsync(int attemptId, int score, int timeTakenInMinutes);
}

