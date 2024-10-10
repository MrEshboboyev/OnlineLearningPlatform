using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class AnswerService(IUnitOfWork unitOfWork, IMapper mapper) : IAnswerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region Answer Management
    public async Task<ResponseDTO<IEnumerable<AnswerDTO>>> GetAllAnswersAsync()
    {
        try
        {
            var allAnswers = await _unitOfWork.Answer.GetAllAsync(
                includeProperties: "Question");

            var mappedAnswers = _mapper.Map<IEnumerable<AnswerDTO>>(allAnswers);

            return new ResponseDTO<IEnumerable<AnswerDTO>>(mappedAnswers);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<AnswerDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<AnswerDTO>>> GetAnswersByQuestionIdAsync(int questionId)
    {
        try
        {
            var questionAnswers = await _unitOfWork.Answer.GetAllAsync(
                filter: a => a.QuestionId.Equals(questionId),
                includeProperties: "Question");

            var mappedAnswers = _mapper.Map<IEnumerable<AnswerDTO>>(questionAnswers);

            return new ResponseDTO<IEnumerable<AnswerDTO>>(mappedAnswers);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<AnswerDTO>>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> AddAnswerToQuestionAsync(int questionId, AnswerDTO answerDTO)
    {
        try
        {
            var question = await _unitOfWork.Question.GetAsync(
                filter: q => q.Id.Equals(questionId),
                includeProperties: "Answers"
                ) ?? throw new Exception("Question not found!");

            // prepare answer
            var answerForDb = _mapper.Map<Answer>(answerDTO);

            question.Answers.Add(answerForDb);

            await _unitOfWork.Question.UpdateAsync(question);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UpdateAnswerAsync(AnswerDTO answerDTO)
    {
        try
        {
            var answerFromDb = await _unitOfWork.Answer.GetAsync(
                a => a.Id.Equals(answerDTO.Id)
                ) ?? throw new Exception("Answer not found!");

            // mapping fields
            _mapper.Map(answerDTO, answerFromDb);

            await _unitOfWork.Answer.UpdateAsync(answerFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> DeleteAnswerFromQuestionAsync(int answerId)
    {
        try
        {
            var answerFromDb = await _unitOfWork.Answer.GetAsync(
                a => a.Id.Equals(answerId)
                ) ?? throw new Exception("Answer not found!");

            await _unitOfWork.Answer.RemoveAsync(answerFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Answer Correctness
    public async Task<ResponseDTO<bool>> IsCorrectAnswerAsync(int answerId)
    {
        try
        {
            var answerFromDb = await _unitOfWork.Answer.GetAsync(
                a => a.Id.Equals(answerId)
                ) ?? throw new Exception("Answer not found!");

            return new ResponseDTO<bool>(answerFromDb.IsCorrect);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<bool>> SetCorrectAnswerAsync(int answerId)
    {
        try
        {
            var answerFromDb = await _unitOfWork.Answer.GetAsync(
                a => a.Id.Equals(answerId)
                ) ?? throw new Exception("Answer not found!");

            // updated to correct
            answerFromDb.IsCorrect = true;

            await _unitOfWork.Answer.UpdateAsync(answerFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<bool>(answerFromDb.IsCorrect);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    #endregion

    #region Answer Validation
    public async Task<ResponseDTO<bool>> ValidateAnswerAsync(int answerId)
    {
        try
        {
            // Step 1: Retrieve the answer by its ID
            var answer = await _unitOfWork.Answer.GetAsync(
                filter: a => a.Id.Equals(answerId),
                includeProperties: "Question"
                ) ?? throw new Exception("Answer not found!");

            // Step 2: Check if the answer exists and is valid (i.e., has non-empty text and is associated with a valid question)
            bool isValid = (!string.IsNullOrWhiteSpace(answer.AnswerText) && answer.Question is not null);

            // Step 3: Return the result
            return new ResponseDTO<bool>(isValid);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    #endregion
}
