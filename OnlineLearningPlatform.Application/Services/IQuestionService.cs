using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface IQuestionService
{
    #region Question Management
    Task<ResponseDTO<IEnumerable<QuizDTO>>> GetAllQuestionsAsync();
    Task<ResponseDTO<IEnumerable<QuizDTO>>> GetQuestionsByQuizIdAsync(int quizId);
    Task<ResponseDTO<QuizDTO>> GetQuestionByIdAsync(int questionId);

    // POST actions
    Task<ResponseDTO<object>> CreateQuestionAsync(QuestionDTO questionDTO);
    Task<ResponseDTO<object>> UpdateQuestionAsync(QuestionDTO questionDTO);
    Task<ResponseDTO<object>> DeleteQuestionAsync(QuestionDTO questionDTO);
    #endregion

    #region Question Type and Metadata
    Task<ResponseDTO<string>> GetQuestionTypeAsync(int questionId);

    // POST actions
    Task<ResponseDTO<string>> UpdateQuestionTypeAsync(int questionId, string questionType);
    #endregion

    #region Question Answer Validation
    Task<ResponseDTO<bool>> ValidateQuestionAsync(int questionId);
    #endregion

    #region Question Status
    Task<ResponseDTO<bool>> IsQuestionUsedInQuizAsync(int questionId);
    #endregion

    #region Question Duplication
    // POST actions
    Task<ResponseDTO<object>> DuplicateQuestionAsync(int questionId);
    #endregion

    #region Question Order
    Task<ResponseDTO<object>> SetQuestionOrderAsync(int quizId, Dictionary<int, int> questionOrderMap);
    #endregion
}
