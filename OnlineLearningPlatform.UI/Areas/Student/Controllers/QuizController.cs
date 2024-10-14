using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.UI.Controllers;

namespace OnlineLearningPlatform.UI.Areas.Student.Controllers
{
    [Area(SD.Role_Student)]
    [Authorize(Roles = SD.Role_Student)]
    [Route($"{SD.Role_Student}/Quiz")]
    public class QuizController(IQuizService quizService,
        IAnswerService answerService,
        IQuizSubmissionService quizSubmissionService) : BaseController
    {
        private readonly IQuizService _quizService = quizService;
        private readonly IAnswerService _answerService = answerService;
        private readonly IQuizSubmissionService _quizSubmissionService = quizSubmissionService;

        [HttpGet("{courseId}")]
        public async Task<IActionResult> Index(int courseId)
        {
            var courseQuizzes = (await _quizService.GetQuizzesByCourseIdAsync(courseId)).Data;
            return View(courseQuizzes);
        }

        [HttpGet("TakeQuiz/{quizId}")]
        public async Task<IActionResult> TakeQuiz(int quizId)
        {
            var quizQuestions = (await _quizService.GetQuestionsByQuizAsync(quizId)).Data;
            ViewBag.QuizId = quizId;
            return View(quizQuestions);
        }

        [HttpPost("SubmitQuiz/{quizId}")]
        public async Task<IActionResult> SubmitQuiz(int quizId, Dictionary<int, int> answers)
        {
            if (answers == null || !answers.Any())
            {
                TempData["error"] = "You must select an answer for each question.";
                return RedirectToAction(nameof(TakeQuiz), new { quizId });
            }

            var correctAnswersCount = 0;

            foreach (var answer in answers)
            {
                // answer.Key is the questionId, answer.Value is the selected answerId
                if ((await _answerService.IsCorrectAnswerAsync(answer.Value)).Data)
                {
                    correctAnswersCount++;
                }
            }

            var allAnswersCount = answers.Count;

            QuizSubmissionDTO quizSubmissionDTO = new()
            {
                QuizId = quizId,
                StudentId = GetUserId(),
                SubmissionDate = DateTime.Now,
                Grade = ((double)correctAnswersCount / allAnswersCount) * 10
            };

            // Get quiz to retrieve courseId
            var quiz = (await _quizService.GetQuizByIdAsync(quizId)).Data;

            // Submit the quiz
            var result = await _quizSubmissionService.SubmitQuizAsync(quizSubmissionDTO);

            if (result.Success)
            {
                TempData["success"] = $"Quiz successfully submitted! Your grade is {quizSubmissionDTO.Grade}.";
                return RedirectToAction(nameof(Index), new { courseId = quiz.CourseId });
            }

            TempData["error"] = $"Error: {result.Message}";
            return RedirectToAction(nameof(TakeQuiz), new { quizId });
        }

        [HttpGet("QuizResults")]
        public async Task<IActionResult> QuizResults()
        {
            var quizSubmissions = (await _quizSubmissionService.GetSubmissionsByStudentIdAsync(GetUserId())).Data;
            return View(quizSubmissions);
        }
    }
}
