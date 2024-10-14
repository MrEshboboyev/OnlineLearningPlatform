using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class QuizSubmissionService(IUnitOfWork unitOfWork, IMapper mapper) : IQuizSubmissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region Quiz Submission Management
    // POST actions
    public async Task<ResponseDTO<object>> SubmitQuizAsync(QuizSubmissionDTO quizSubmissionDTO)
    {
        try
        {
            var quizSubmission = _mapper.Map<QuizSubmission>(quizSubmissionDTO);

            await _unitOfWork.QuizSubmission.AddAsync(quizSubmission);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Quiz submitted!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UpdateQuizSubmissionAsync(int submissionId, QuizSubmissionDTO quizSubmissionDTO)
    {
        try
        {
            var quizSubmissionFromDb = await _unitOfWork.QuizSubmission.GetAsync(
                qs => qs.Id.Equals(submissionId)
                ) ?? throw new Exception("Quiz submission not found!");

            // mapping fields
            _mapper.Map(quizSubmissionDTO, quizSubmissionFromDb);

            // update and save
            await _unitOfWork.QuizSubmission.UpdateAsync(quizSubmissionFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> DeleteQuizSubmissionAsync(int submissionId)
    {
        try
        {
            var quizSubmissionFromDb = await _unitOfWork.QuizSubmission.GetAsync(
                qs => qs.Id.Equals(submissionId)
                ) ?? throw new Exception("Quiz submission not found!");

            // update and save
            await _unitOfWork.QuizSubmission.RemoveAsync(quizSubmissionFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Quiz Grading
    public async Task<ResponseDTO<bool>> IsQuizSubmissionGradeAsync(int submissionId)
    {
        try
        {
            var quizSubmissionFromDb = await _unitOfWork.QuizSubmission.GetAsync(
                qs => qs.Id.Equals(submissionId)
                ) ?? throw new Exception("Quiz submission not found!");

            var isQuizSubmissionGrade = quizSubmissionFromDb.Grade > 0;

            return new ResponseDTO<bool>(isQuizSubmissionGrade);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<double>> GradeQuizAsync(int submissionId)
    {
        try
        {
            // Step 1: Retrieve the quiz submission with its associated quiz and answers
            var submission = await _unitOfWork.QuizSubmission.GetAsync(
                filter: q => q.Id.Equals(submissionId),
                includeProperties: "Quiz.Question.Answers"
                ) ?? throw new Exception("Quiz submission not found!");

            // Step 2: Calculate the score (assumes multiple-choice, true/false, etc.)
            double totalQuestions = submission.Quiz.Questions.Count;
            double correctAnswers = 0;

            foreach (var question in submission.Quiz.Questions)
            {
                // Find the correct answer(s) for each question
                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);

                // Assuming we store user's answers somewhere, for now we assume this logic
                // if submission contains user answer comparison logic, implement here
                if (correctAnswer != null)
                {
                    // Validate if the user's answer matches the correct answer (replace this with actual logic)
                    bool isAnswerCorrect = true; // Placeholder, use real comparison logic here.

                    if (isAnswerCorrect)
                    {
                        correctAnswers++;
                    }
                }
            }

            // Step 3: Calculate the final grade as a percentage
            double grade = (correctAnswers / totalQuestions) * 100;

            // Return the grade
            return new ResponseDTO<double>(grade);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }
    public async Task<ResponseDTO<bool>> UpdateQuizGradeAsync(int submissionId, double newGrade)
    {
        try
        {
            // Step 1: Retrieve the quiz submission
            var submission = await _unitOfWork.QuizSubmission.GetAsync(
                q => q.Id.Equals(submissionId)
                ) ?? throw new Exception("Quiz submission not found!");

            // Step 2: Update the grade
            submission.Grade = newGrade;

            // Step 3: Update and Save changes
            await _unitOfWork.QuizSubmission.UpdateAsync(submission);
            await _unitOfWork.SaveAsync();

            // Return success response
            return new ResponseDTO<bool>(true);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    #endregion

    #region Quiz Submission Retrieval
    public async Task<ResponseDTO<QuizSubmissionDTO>> GetQuizSubmissionByIdAsync(int submissionId)
    {
        try
        {
            var quizSubmissionFromDb = await _unitOfWork.QuizSubmission.GetAsync(
               qs => qs.Id.Equals(submissionId)
               ) ?? throw new Exception("Quiz submission not found!");

            var mappedSubmission = _mapper.Map<QuizSubmissionDTO>(quizSubmissionFromDb);

            return new ResponseDTO<QuizSubmissionDTO>(mappedSubmission);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<QuizSubmissionDTO>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetSubmissionsByQuizIdAsync(int quizId)
    {
        try
        {
            var quizSubmissionsFromDb = await _unitOfWork.QuizSubmission.GetAllAsync(
               qs => qs.QuizId.Equals(quizId));

            var mappedSubmissions = _mapper.Map<IEnumerable<QuizSubmissionDTO>>(quizSubmissionsFromDb);

            return new ResponseDTO<IEnumerable<QuizSubmissionDTO>>(mappedSubmissions);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuizSubmissionDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetSubmissionsByStudentIdAsync(string studentId)
    {
        try
        {
            var studentQuizSubmissionsFromDb = await _unitOfWork.QuizSubmission.GetAllAsync(
               filter: qs => qs.StudentId.Equals(studentId),
               includeProperties: "Quiz");

            var mappedSubmissions = _mapper.Map<IEnumerable<QuizSubmissionDTO>>(studentQuizSubmissionsFromDb);

            return new ResponseDTO<IEnumerable<QuizSubmissionDTO>>(mappedSubmissions);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuizSubmissionDTO>>(ex.Message);
        }
    }
    #endregion

    #region Submission Validation
    public async Task<ResponseDTO<bool>> HasStudentSubmittedQuizAsync(int quizId, string studentId)
    {
        try
        {
            var existStudentQuizSubmissionFromDb = await _unitOfWork.QuizSubmission.AnyAsync(
               qs => qs.QuizId.Equals(quizId) && qs.StudentId.Equals(studentId));

            return new ResponseDTO<bool>(existStudentQuizSubmissionFromDb);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    #endregion

    #region Submission History and Statistics
    public async Task<ResponseDTO<IEnumerable<QuizSubmissionDTO>>> GetQuizSubmissionHistoryAsync(string studentId)
    {
        try
        {
            var studentQuizSubmissionsFromDb = await _unitOfWork.QuizSubmission.GetAllAsync(
               qs => qs.StudentId.Equals(studentId));

            var mappedSubmissions = _mapper.Map<IEnumerable<QuizSubmissionDTO>>(studentQuizSubmissionsFromDb);

            return new ResponseDTO<IEnumerable<QuizSubmissionDTO>>(mappedSubmissions);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<QuizSubmissionDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<double>> GetAverageGradeForQuizAsync(int quizId)
    {
        try
        {
            var quizSubmissionsFromDb = await _unitOfWork.QuizSubmission.GetAllAsync(
               qs => qs.QuizId.Equals(quizId));

            var averageGrade = quizSubmissionsFromDb.Average(qs => qs.Grade);

            return new ResponseDTO<double>(averageGrade);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }
    public async Task<ResponseDTO<double>> GetStudentAverageQuizGradeAsync(string studentId)
    {
        try
        {
            var studentSubmissionsFromDb = await _unitOfWork.QuizSubmission.GetAllAsync(
               qs => qs.StudentId.Equals(studentId));

            var averageGrade = studentSubmissionsFromDb.Average(qs => qs.Grade);

            return new ResponseDTO<double>(averageGrade);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }
    #endregion
}

