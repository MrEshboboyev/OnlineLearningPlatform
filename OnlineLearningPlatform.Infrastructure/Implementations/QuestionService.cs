using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class QuestionService(IUnitOfWork unitOfWork, IMapper mapper) : IQuestionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region Question Management
    public async Task<ResponseDTO<IEnumerable<QuestionDTO>>> GetAllQuestionsAsync()
    {
        try
        {
            var allQuestions = await _unitOfWork.Question.GetAllAsync(
                includeProperties: "Quiz,Answers");

            var mappedQuestions = _mapper.Map<IEnumerable<QuestionDTO>>(allQuestions);

            return new ResponseDTO<IEnumerable<QuestionDTO>>(mappedQuestions);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuestionDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<QuestionDTO>>> GetQuestionsByInstructorAsync(string instructorId)
    {
        try
        {
            var instructorQuestions = await _unitOfWork.Question.GetAllAsync(
                filter: q => q.Quiz.Course.InstructorId.Equals(instructorId),
                includeProperties: "Quiz,Answers");

            var mappedQuestions = _mapper.Map<IEnumerable<QuestionDTO>>(instructorQuestions);

            return new ResponseDTO<IEnumerable<QuestionDTO>>(mappedQuestions);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuestionDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<QuestionDTO>>> GetQuestionsByQuizIdAsync(int quizId)
    {
        try
        {
            var quizQuestions = await _unitOfWork.Question.GetAllAsync(
                filter: q => q.QuizId.Equals(quizId),
                includeProperties: "Quiz,Answers");

            var mappedQuestions = _mapper.Map<IEnumerable<QuestionDTO>>(quizQuestions);

            return new ResponseDTO<IEnumerable<QuestionDTO>>(mappedQuestions);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuestionDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<QuestionDTO>> GetQuestionByIdAsync(int questionId)
    {
        try
        {
            var question = await _unitOfWork.Question.GetAsync(
                filter: q => q.Id.Equals(questionId),
                includeProperties: "Quiz,Answers");

            var mappedQuestion = _mapper.Map<QuestionDTO>(question);

            return new ResponseDTO<QuestionDTO>(mappedQuestion);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<QuestionDTO>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> CreateQuestionAsync(QuestionDTO questionDTO)
    {
        try
        {
            var questionForDb = _mapper.Map<Question>(questionDTO);

            await _unitOfWork.Question.AddAsync(questionForDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Question created!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UpdateQuestionAsync(QuestionDTO questionDTO)
    {
        try
        {
            var questionFromDb = await _unitOfWork.Question.GetAsync(
                filter: q => q.Id.Equals(questionDTO.Id)
                ) ?? throw new Exception("Question not found!");

            // mapping fields
            _mapper.Map(questionDTO, questionFromDb);

            await _unitOfWork.Question.UpdateAsync(questionFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Question updated!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> DeleteQuestionAsync(QuestionDTO questionDTO)
    {
        try
        {
            var questionFromDb = await _unitOfWork.Question.GetAsync(
                filter: q => q.Id.Equals(questionDTO.Id)
                ) ?? throw new Exception("Question not found!");

            await _unitOfWork.Question.RemoveAsync(questionFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Question deleted!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Question Type and Metadata
    public async Task<ResponseDTO<string>> GetQuestionTypeAsync(int questionId)
    {
        try
        {
            var question = await _unitOfWork.Question.GetAsync(
                q => q.Id.Equals(questionId)
                ) ?? throw new Exception("Question not found!");

            return new ResponseDTO<string>(question.QuestionType);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<string>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<string>> UpdateQuestionTypeAsync(int questionId, string questionType)
    {
        try
        {
            var question = await _unitOfWork.Question.GetAsync(
                q => q.Id.Equals(questionId)
                ) ?? throw new Exception("Question not found!");

            // update question type
            question.QuestionType = questionType;

            await _unitOfWork.Question.UpdateAsync(question);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<string>(question.QuestionType);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<string>(ex.Message);
        }
    }
    #endregion

    #region Question Answer Validation
    public async Task<ResponseDTO<bool>> ValidateQuestionAsync(int questionId)
    {
        try
        {
            // Step 1: Retrieve the question
            var question = await _unitOfWork.Question.GetAsync(
                q => q.Id.Equals(questionId));

            // Step 2: Check if question exists and has valid text
            bool isValid = (question != null && !string.IsNullOrWhiteSpace(question.QuestionText));

            // Step 3: Return the result
            return new ResponseDTO<bool>(isValid);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    #endregion

    #region Question Status
    public async Task<ResponseDTO<bool>> IsQuestionUsedInQuizAsync(int questionId)
    {
        try
        {
            // Step 1: Retrieve the question
            var question = await _unitOfWork.Question.GetAsync(
                q => q.Id.Equals(questionId));

            // Step 2: Check if the question is part of a quiz
            bool isUsedInQuiz = (question != null && question.QuizId > 0);

            // Step 3: Return the result
            return new ResponseDTO<bool>(isUsedInQuiz);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }

    #endregion

    #region Question Order
    public async Task<ResponseDTO<object>> SetQuestionOrderAsync(int quizId, Dictionary<int, int> questionOrderMap)
    {
        try
        {
            // Step 1: Retrieve the questions for the specified quiz
            var questions = await _unitOfWork.Question.GetAllAsync(
                q => q.QuizId.Equals(quizId));

            // Step 2: Update the question order based on the map provided
            foreach (var question in questions)
            {
                if (questionOrderMap.TryGetValue(question.Id, out int value))
                {
                    question.QuestionOrder = value;
                }
            }

            // Step 3: Save changes to the database
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region IQuestionService and IAnswerService Combined
    public async Task<ResponseDTO<bool>> ValidateCorrectAnswerExistsForQuestionAsync(int questionId)
    {
        try
        {
            var question = await _unitOfWork.Question.GetAsync(
                filter: q => q.Id.Equals(questionId),
                includeProperties: "Answers"
                ) ?? throw new Exception("Question not found!");

            bool hasCorrectAnswer = question.Answers.Any(a => a.IsCorrect);

            return new ResponseDTO<bool>(hasCorrectAnswer);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    #endregion
}

