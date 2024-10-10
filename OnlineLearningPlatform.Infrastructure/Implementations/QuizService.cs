using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;
using System.Linq.Expressions;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class QuizService(IUnitOfWork unitOfWork, IMapper mapper) : IQuizService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region Quiz Management
    public async Task<ResponseDTO<IEnumerable<QuizDTO>>> GetAllQuizzesAsync()
    {
        try
        {
            var allQuizzes = await _unitOfWork.Quiz.GetAllAsync(
                includeProperties: "Course,Questions,QuizSubmissions");

            var mappedQuizzes = _mapper.Map<IEnumerable<QuizDTO>>(allQuizzes);

            return new ResponseDTO<IEnumerable<QuizDTO>>(mappedQuizzes);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuizDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<QuizDTO>>> GetQuizzesByCourseIdAsync(int courseId)
    {
        try
        {
            var courseQuizzes = await _unitOfWork.Quiz.GetAllAsync(
                filter: q => q.CourseId.Equals(courseId),
                includeProperties: "Course,Questions,QuizSubmissions");

            var mappedQuizzes = _mapper.Map<IEnumerable<QuizDTO>>(courseQuizzes);

            return new ResponseDTO<IEnumerable<QuizDTO>>(mappedQuizzes);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuizDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<QuizDTO>> GetQuizByIdAsync(int quizId)
    {
        try
        {
            var quiz = await _unitOfWork.Quiz.GetAsync(
                filter: q => q.Id.Equals(quizId),
                includeProperties: "Course,Questions,QuizSubmissions");

            var mappedQuiz = _mapper.Map<QuizDTO>(quiz);

            return new ResponseDTO<QuizDTO>(mappedQuiz);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<QuizDTO>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> CreateQuizAsync(QuizDTO quizDTO)
    {
        try
        {
            var quizForDb = _mapper.Map<Quiz>(quizDTO);

            await _unitOfWork.Quiz.AddAsync(quizForDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UpdateQuizAsync(QuizDTO quizDTO)
    {
        try
        {
            var quizFromDb = await _unitOfWork.Quiz.GetAsync(
                q => q.Id.Equals(quizDTO)
                ) ?? throw new Exception("Quiz not found!");

            // mapping quiz
            _mapper.Map(quizDTO, quizFromDb);

            await _unitOfWork.Quiz.UpdateAsync(quizFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> DeleteQuizAsync(QuizDTO quizDTO)
    {
        try
        {
            var quizFromDb = await _unitOfWork.Quiz.GetAsync(
                q => q.Id.Equals(quizDTO)
                ) ?? throw new Exception("Quiz not found!");

            await _unitOfWork.Quiz.RemoveAsync(quizFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Question Management within Quizzes
    public async Task<ResponseDTO<IEnumerable<QuestionDTO>>> GetQuestionsByQuizAsync(int quizId)
    {
        try
        {
            var quizQuestions = await _unitOfWork.Question.GetAllAsync(
                q => q.QuizId.Equals(quizId), 
                includeProperties: "Quiz,Answers");

            var mappedQuestions = _mapper.Map<IEnumerable<QuestionDTO>>(quizQuestions);

            return new ResponseDTO<IEnumerable<QuestionDTO>>(mappedQuestions);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuestionDTO>>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> AddQuestionToQuizAsync(int quizId, int questionId)
    {
        try
        {
            // get quiz with questions
            var quizFromDb = await _unitOfWork.Quiz.GetAsync(
                filter: q => q.Id.Equals(quizId), 
                includeProperties: "Questions"
                ) ?? throw new Exception("Quiz not found!");

            // get question to add to quiz
            var questionFromDb = await _unitOfWork.Question.GetAsync(
                q => q.Id.Equals(questionId)
                ) ?? throw new Exception("Question not found!");

            // adding question to quiz
            quizFromDb.Questions.Add(questionFromDb);

            await _unitOfWork.Quiz.UpdateAsync(quizFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> RemoveQuestionFromQuizAsync(int quizId, int questionId)
    {
        try
        {
            // get quiz with questions
            var quizFromDb = await _unitOfWork.Quiz.GetAsync(
                filter: q => q.Id.Equals(quizId),
                includeProperties: "Questions"
                ) ?? throw new Exception("Quiz not found!");

            // get question to remove from quiz
            var questionFromDb = await _unitOfWork.Question.GetAsync(
                q => q.Id.Equals(questionId)
                ) ?? throw new Exception("Question not found!");

            // adding question to quiz
            quizFromDb.Questions.Remove(questionFromDb);

            await _unitOfWork.Quiz.UpdateAsync(quizFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Quiz Submissions and Attempts
    public async Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetSubmissionsByQuizAsync(int quizId)
    {
        try
        {
            var quizSubmissions = await _unitOfWork.QuizSubmission.GetAllAsync(
                qs => qs.QuizId.Equals(quizId),
                includeProperties: "Quiz,Student");

            var mappedSubmissions = _mapper.Map<IEnumerable<QuizSubmissionDTO>>(quizSubmissions);

            return new ResponseDTO<IEnumerable<QuizSubmissionDTO>>(mappedSubmissions);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuizSubmissionDTO>>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<QuizSubmissionDTO>> SubmitQuizAsync(int quizId, QuizSubmissionDTO quizSubmissionDTO)
    {
        try
        {
            // get quiz
            var quizFromDb = await _unitOfWork.Quiz.GetAsync(
                q => q.Id.Equals(quizId), 
                includeProperties: "QuizSubmission"
                ) ?? throw new Exception("Quiz not found!");

            var quizSubmissionForDb = _mapper.Map<QuizSubmission>(quizSubmissionDTO);

            // adding quiz submission to quiz
            quizFromDb.QuizSubmissions.Add(quizSubmissionForDb);

            // update and save
            await _unitOfWork.Quiz.UpdateAsync(quizFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<QuizSubmissionDTO>(quizSubmissionDTO);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<QuizSubmissionDTO>(ex.Message);
        }
    }
    #endregion

    #region Quiz Grading and Feedback
    public async Task<ResponseDTO<double>> GetQuizScoreAsync(int submissionId)
    {
        try
        {
            var quizSubmission = await _unitOfWork.QuizSubmission.GetAsync(
                qs => qs.Id.Equals(submissionId)
                ) ?? throw new Exception("Quiz submission not found!");

            return new ResponseDTO<double>(quizSubmission.Grade);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> GradeQuizSubmissionAsync(int submissionId, double grade)
    {
        try
        {
            var quizSubmission = await _unitOfWork.QuizSubmission.GetAsync(
                qs => qs.Id.Equals(submissionId)
                ) ?? throw new Exception("Quiz submission not found!");

            // update grade
            quizSubmission.Grade = grade;

            await _unitOfWork.QuizSubmission.UpdateAsync(quizSubmission);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Quiz Status and Validation
    public async Task<ResponseDTO<bool>> IsQuizCompletedAsync(int quizId, string studentId)
    {
        try
        {
            var quizSubmission = await _unitOfWork.QuizSubmission.GetAsync(
                qs => qs.QuizId.Equals(quizId) && qs.StudentId.Equals(studentId)
                ) ?? throw new Exception("Quiz submission not found!");

            var isCompleted = quizSubmission.Grade > 0;

            return new ResponseDTO<bool>(isCompleted);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    public async Task<ResponseDTO<bool>> HasStudentAttemptedQuizAsync(int quizId, string studentId)
    {
        try
        {
            var hasAttempted = await _unitOfWork.QuizSubmission.AnyAsync(
                qs => qs.QuizId.Equals(quizId) && qs.StudentId.Equals(studentId));

            return new ResponseDTO<bool>(hasAttempted);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    #endregion

    #region Quiz Progress Tracking
    public async Task<ResponseDTO<double>> GetStudentProgressInQuizAsync(int quizId, string studentId)
    {
        try
        {
            var studentQuizSubmission = await _unitOfWork.QuizSubmission.GetAsync(
                qs => qs.QuizId.Equals(quizId) && qs.StudentId.Equals(studentId)
                ) ?? throw new Exception("Quiz Submission not found!");

            return new ResponseDTO<double>(studentQuizSubmission.Grade);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }
    #endregion

    #region Quiz Statistics
    public async Task<ResponseDTO<int>> GetTotalSubmissionsForQuizAsync(int quizId)
    {
        try
        {
            var quizSubmissions = await _unitOfWork.QuizSubmission.GetAllAsync(
                qs => qs.QuizId.Equals(quizId));

            return new ResponseDTO<int>(quizSubmissions.Count());
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }
    public async Task<ResponseDTO<double>> GetAverageScoreForQuizAsync(int quizId)
    {
        try
        {
            var quizSubmissions = await _unitOfWork.QuizSubmission.GetAllAsync(
                qs => qs.QuizId.Equals(quizId));

            var averageScore = quizSubmissions.Average(qs => qs.Grade);

            return new ResponseDTO<double>(averageScore);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }
    public async Task<ResponseDTO<Dictionary<string, int>>> GetQuizPerformanceStatsAsync(int quizId)
    {
        try
        {
            // Step 1: Retrieve all quiz submissions for the specified quiz
            var quizSubmissions = await _unitOfWork.QuizSubmission
                .GetAllAsync(qs => qs.QuizId.Equals(quizId));

            // Step 2: Initialize the statistics
            int totalSubmissions = quizSubmissions.Count();
            int passedCount = 0;
            int failedCount = 0;
            double totalGrade = 0;

            // Step 3: Calculate pass/fail counts and total grades
            foreach (var submission in quizSubmissions)
            {
                totalGrade += submission.Grade;

                if (submission.Grade >= 50) 
                {
                    passedCount++;
                }
                else
                {
                    failedCount++;
                }
            }

            // Step 4: Calculate average grade (if any submissions exist)
            double averageGrade = totalSubmissions > 0 ? totalGrade / totalSubmissions : 0;

            // Step 5: Create the dictionary to store stats
            var stats = new Dictionary<string, int>
        {
            { "TotalSubmissions", totalSubmissions },
            { "Passed", passedCount },
            { "Failed", failedCount },
            { "AverageGrade", (int)Math.Round(averageGrade) } // Convert average grade to int for simplicity
        };

            // Step 6: Return the stats
            return new ResponseDTO<Dictionary<string, int>>(stats);
        }
        catch (Exception ex)
        {
            // Handle any exception that may occur
            return new ResponseDTO<Dictionary<string, int>>(ex.Message);
        }
    }

    #endregion
}

