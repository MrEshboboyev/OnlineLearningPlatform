using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface IAnswerService
{
    #region Answer Management
    Task<ResponseDTO<IEnumerable<AnswerDTO>>> GetAllAnswersAsync();
    Task<ResponseDTO<IEnumerable<AnswerDTO>>> GetAnswersByQuestionIdAsync(int questionId);

    // POST actions
    Task<ResponseDTO<object>> AddAnswerToQuestionAsync(int questionId, AnswerDTO answerDTO);
    Task<ResponseDTO<object>> UpdateAnswerAsync(AnswerDTO answerDTO);
    Task<ResponseDTO<object>> DeleteAnswerToQuestionAsync(int answerId);
    #endregion

    #region Answer Correctness
    Task<ResponseDTO<bool>> IsCorrectAnswerAsync(int answerId);

    // POST actions
    Task<ResponseDTO<bool>> SetCorrectAnswerAsync(int answerId);
    #endregion

    #region Answer Feedback
    // POST actions
    Task<ResponseDTO<object>> ProvideFeedbackForAnswerAsync(int answerId, string feedback);
    #endregion

    #region Answer Validation
    Task<ResponseDTO<bool>> ValidateAnswerAsync(int answerId);
    #endregion

    #region Answer Duplication
    // POST actions
    Task<ResponseDTO<object>> DuplicateAnswerAsync(int answer);
    #endregion
}
