using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.UI.Controllers;

namespace OnlineLearningPlatform.UI.Areas.Instructor.Controllers
{
    [Area(SD.Role_Instructor)]
    [Authorize(Roles = SD.Role_Instructor)]
    [Route($"{SD.Role_Instructor}/Quiz")]
    public class QuizController(IQuizService quizService,
        IQuizSubmissionService quizSubmissionService,
        ICourseService courseService) : BaseController
    {
        private readonly IQuizService _quizService = quizService;
        private readonly IQuizSubmissionService _quizSubmissionService = quizSubmissionService;
        private readonly ICourseService _courseService = courseService;

        public async Task<IActionResult> Index()
        {
            var instructorQuizzes = (await _quizService.GetQuizzesByInstructorAsync(GetUserId())).Data;
            return View(instructorQuizzes);
        }

        [HttpGet("ManageQuizzes/{courseId}")]
        public async Task<IActionResult> ManageQuizzes(int courseId)
        {
            var courseQuizzes = (await _quizService.GetQuizzesByCourseIdAsync(courseId)).Data;

            ViewBag.CourseId = courseId;
            return View(courseQuizzes);
        }

        [HttpGet("QuizDetails/{quizId}")]
        public async Task<IActionResult> QuizDetails(int quizId)
        {
            var quiz = (await _quizService.GetQuizByIdAsync(quizId)).Data;
            return View(quiz);
        }

        [HttpGet("CreateQuiz/{courseId}")]
        public async Task<IActionResult> CreateQuiz(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;

            ViewBag.CourseTitle = course.Title;
            ViewBag.CourseId = courseId;

            return View();
        }

        [HttpPost("CreateQuiz/{courseId}")]
        public async Task<IActionResult> CreateQuiz(QuizDTO quizDTO)
        {
            var result = await _quizService.CreateQuizAsync(quizDTO);

            if (result.Success)
            {
                TempData["success"] = "Quiz created successfully!";
                return RedirectToAction(nameof(ManageQuizzes), new { courseId = quizDTO.CourseId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return RedirectToAction(nameof(CreateQuiz), new { courseId = quizDTO.CourseId });
        }

        [HttpGet("EditQuiz/{quizId}")]
        public async Task<IActionResult> EditQuiz(int quizId)
        {
            var quiz = (await _quizService.GetQuizByIdAsync(quizId)).Data;
            return View(quiz);
        }

        [HttpPost("EditQuiz/{quizId}")]
        public async Task<IActionResult> EditQuiz(QuizDTO quizDTO)
        {
            var result = await _quizService.UpdateQuizAsync(quizDTO);

            if (result.Success)
            {
                TempData["success"] = "Quiz updated successfully!";
                return RedirectToAction(nameof(ManageQuizzes), new { courseId = quizDTO.CourseId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return RedirectToAction(nameof(EditQuiz), new { quizId = quizDTO.Id });
        }


        [HttpGet("DeleteQuiz/{quizId}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            var quiz = (await _quizService.GetQuizByIdAsync(quizId)).Data;
            return View(quiz);
        }

        [HttpPost("DeleteQuiz/{quizId}")]
        public async Task<IActionResult> DeleteQuiz(QuizDTO quizDTO)
        {
            var result = await _quizService.DeleteQuizAsync(quizDTO);

            if (result.Success)
            {
                TempData["success"] = "Quiz deleted successfully!";
                return RedirectToAction(nameof(ManageQuizzes), new { courseId = quizDTO.CourseId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return RedirectToAction(nameof(EditQuiz), new { quizId = quizDTO.Id });
        }
    }
}
