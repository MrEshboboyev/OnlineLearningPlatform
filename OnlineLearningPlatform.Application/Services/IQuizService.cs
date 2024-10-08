using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface IQuizService
{
    #region Quiz Management
    Task<ResponseDTO<IEnumerable<QuizDTO>>> GetAllQuizzesAsync();
    Task<ResponseDTO<IEnumerable<QuizDTO>>> GetQuizzesByCourseIdAsync(int courseId);
    Task<ResponseDTO<QuizDTO>> GetQuizByIdAsync(int courseId);

    // POST actions
    Task<ResponseDTO<object>> CreateQuizAsync(QuizDTO quizDTO);
    Task<ResponseDTO<object>> UpdateQuizAsync(QuizDTO quizDTO);
    Task<ResponseDTO<object>> DeleteQuizAsync(QuizDTO quizDTO);
    #endregion

    #region Question Management within Quizzes
    Task<ResponseDTO<IEnumerable<QuestionDTO>>> GetQuestionsByQuizAsync(int quizId);

    // POST actions
    Task<ResponseDTO<object>> AddQuestionToQuizAsync(int quizId, int questionId);
    Task<ResponseDTO<object>> RemoveQuestionFromQuizAsync(int quizId, int questionId);
    Task<ResponseDTO<object>> UpdateQuestionToQuizAsync(int quizId, int questionId, QuestionDTO questionDTO);
    #endregion

    #region Quiz Submissions and Attempts
    Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetSubmissionsByQuizAsync(int quizId);
    Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetSubmissionsByQuizAsync(int quizId);
    Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetSubmissionsByQuizAsync(int quizId);

    // POST actions
    Task<ResponseDTO<QuizSubmissionDTO>> SubmitQuizAsync(int quizId, QuizSubmissionDTO quizSubmissionDTO);
    #endregion

    #region Quiz Grading and Feedback
    Task<ResponseDTO<double>> GetQuizScoreAsync(int submissionId);

    // POST actions
    Task<ResponseDTO<object>> ProvideFeedbackForQuizSubmissionAsync(int submissionId, string feedback);
    Task<ResponseDTO<object>> GradeQuizSubmissionAsync(int submissionId, double grade);
    #endregion

    #region Quiz Status and Validation
    Task<ResponseDTO<bool>> IsQuizCompletedAsync(int quizId, string studentId);
    Task<ResponseDTO<bool>> HasStudentAttemptedQuizAsync(int quizId, string studentId);
    #endregion

    #region Quiz Time Limits and Attempts
    Task<ResponseDTO<int>> GetRemainingAttemptsAsync(int quizId, string studentId);

    // POST actions
    Task<ResponseDTO<object>> SetQuizTimeLimitAsync(int quizId, TimeSpan timeLimit);
    Task<ResponseDTO<object>> SetMaxAttemptsAsync(int quizId, int maxAttempts);
    #endregion

    #region Quiz Activation and Visibility
    Task<ResponseDTO<bool>> IsQuizPublishedAsync(int quizId);

    // POST actions
    Task<ResponseDTO<object>> PublishQuizAsync(int quizId);
    Task<ResponseDTO<object>> UnpublishQuizAsync(int quizId);
    #endregion

    #region Quiz Progress Tracking
    Task<ResponseDTO<double>> GetStudentProgressInQuizAsync(int quizId, string studentId);
    #endregion

    #region Quiz Duplication
    // POST actions
    Task<ResponseDTO<QuizDTO>> DuplicateQuizAsync(int quizId);
    #endregion

    #region Quiz Statistics
    Task<ResponseDTO<int>> GetTotalSubmissionsForQuizAsync(int quizId);
    Task<ResponseDTO<double>> GetAverageScoreForQuizAsync(int quizId);
    Task<ResponseDTO<Dictionary<string, int>>> GetQuizPerformanceStatsAsync(int quizId);
    #endregion
}
