using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface IQuizSubmissionService
{
    #region Quiz Submission Management
    // POST actions
    Task<ResponseDTO<object>> SubmitQuizAsync(QuizSubmissionDTO quizSubmissionDTO);
    Task<ResponseDTO<object>> UpdateQuizSubmissionAsync(int submissionId, QuizSubmissionDTO quizSubmissionDTO);
    Task<ResponseDTO<object>> DeleteQuizSubmissionAsync(int submissionId);
    #endregion

    #region Quiz Grading
    Task<ResponseDTO<bool>> IsQuizSubmissionGradeAsync(int submissionId);

    // POST actions
    Task<ResponseDTO<double>> GradeQuizAsync(int submissionId);
    Task<ResponseDTO<bool>> UpdateQuizGradeAsync(int submissionId, double newGrade);
    #endregion

    #region Quiz Submission Retrieval
    Task<ResponseDTO<QuizSubmissionDTO>> GetQuizSubmissionByIdAsync(int submissionId);
    Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetSubmissionsByQuizIdAsync(int quizId);
    Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetSubmissionsByStudentIdAsync(string studentId);
    #endregion

    #region Submission Validation
    Task<ResponseDTO<bool>> HasStudentSubmittedQuizAsync(int quizId, string studentId);
    Task<ResponseDTO<bool>> IsQuizSubmissionLateAsync(int submissionId);
    #endregion

    #region Submission History and Statistics
    Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetQuizSubmissionHistoryAsync(string studentId);
    Task<ResponseDTO<double>> GetAverageGradeForQuizAsync(int quizId);
    Task<ResponseDTO<double>> GetStudentAverageQuizGradeAsync(string studentId);
    #endregion

    #region Quiz Retakes and Attempts
    Task<ResponseDTO<int>> GetQuizAttemptCountAsync(int quizId, string studentId);

    // POST actions
    Task<ResponseDTO<bool>> AllowQuizRetakeAsync(int quizId, string studentId);
    #endregion

    #region Submission Deadlines and Timing
    Task<ResponseDTO<bool>> CheckQuizSubmissionDeadlineAsync(int quizId);
    #endregion
}
