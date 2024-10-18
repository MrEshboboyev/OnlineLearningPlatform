using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class QuizAttemptService(IUnitOfWork unitOfWork, IMapper mapper) : IQuizAttemptService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    // Gets the quiz attempt by ID
    public async Task<ResponseDTO<QuizAttemptDTO>> GetQuizAttemptByIdAsync(int attemptId)
    {
        try
        {
            var quizAttempt = await _unitOfWork.QuizAttempt.GetAsync(
                filter: qa => qa.Id.Equals(attemptId),
                includeProperties: "Quiz,User");

            var mappedAttempt = _mapper.Map<QuizAttemptDTO>(quizAttempt);   

            return new ResponseDTO<QuizAttemptDTO>(mappedAttempt);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<QuizAttemptDTO>(ex.Message);
        }
    }

    // Gets all attempts for a specific quiz and user
    public async Task<ResponseDTO<IEnumerable<QuizAttemptDTO>>> GetQuizAttemptsForUserAsync(int quizId, string userId)
    {
        try
        {
            var quizAttemptsFromDb = await _unitOfWork.QuizAttempt.GetAllAsync(
                filter: qa => qa.UserId.Equals(userId) && qa.QuizId.Equals(quizId),
                includeProperties: "User,Quiz");

            var mappedAttempts = _mapper.Map<IEnumerable<QuizAttemptDTO>>(quizAttemptsFromDb);

            return new ResponseDTO<IEnumerable<QuizAttemptDTO>>(mappedAttempts);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuizAttemptDTO>>(ex.Message);
        }
    }

    // Checks if the user is allowed to retake the quiz
    public async Task<ResponseDTO<bool>> CanRetakeQuizAsync(int quizId, string userId)
    {
        try
        {
            var quizFromDb = await _unitOfWork.Quiz.GetAsync(
                q => q.Id.Equals(quizId)
                ) ?? throw new Exception("Quiz not found!");

            var userAttemptsForQuiz = await _unitOfWork.QuizAttempt.GetAllAsync(
                qa => qa.QuizId.Equals(quizId) && qa.UserId.Equals(userId));

            var result = quizFromDb.AllowedRetakes == 0 ||
                userAttemptsForQuiz.Count() < quizFromDb.AllowedRetakes;

            return new ResponseDTO<bool>(result);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }

    // Starts a new quiz attempt for a user
    public async Task<ResponseDTO<QuizAttemptDTO>> StartQuizAttemptAsync(int quizId, string userId)
    {
        var quizFromDb = await _unitOfWork.Quiz.GetAsync(
                q => q.Id.Equals(quizId)
                ) ?? throw new Exception("Quiz not found!");

        if (!(await CanRetakeQuizAsync(quizId, userId)).Data)
            throw new Exception("You have reached the retake limit for this quiz.");

        // create quiz
        QuizAttempt attempt = new()
        {
            QuizId = quizId,
            UserId = userId,
            AttemptDate = DateTime.Now,
            TimeTakenInMinutes = 0, // Will be set upon submission
            Score = 0, // Will be set upon submission
            IsTimedOut = false
        };

        await _unitOfWork.QuizAttempt.AddAsync(attempt);
        await _unitOfWork.SaveAsync();

        return new ResponseDTO<QuizAttemptDTO>(null, "Started quiz!");
    }

    // Submits the quiz attempt and calculates the time taken and score
    public async Task<ResponseDTO<object>> SubmitQuizAttemptAsync(int attemptId, int score, int timeTakenInMinutes)
    {
        try
        {
            var attempt = await _unitOfWork.QuizAttempt.GetAsync(
                qa => qa.Id.Equals(attemptId)
                ) ?? throw new Exception("Attempt not found!");

            attempt.Score = score;
            attempt.TimeTakenInMinutes = timeTakenInMinutes;

            var quiz = await _unitOfWork.Quiz.GetAsync(
                q => q.Id.Equals(attempt.QuizId)
                ) ?? throw new Exception("Quiz not found!");

            if (quiz.TimeLimitInMinutes.HasValue && timeTakenInMinutes > quiz.TimeLimitInMinutes.Value)
            {
                attempt.IsTimedOut = true;
            }

            await _unitOfWork.QuizAttempt.UpdateAsync(attempt);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Quiz submitted!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
}
