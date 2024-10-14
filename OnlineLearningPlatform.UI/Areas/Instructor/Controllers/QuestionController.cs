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
    [Route($"{SD.Role_Instructor}/Question")]
    public class QuestionController(IQuizService quizService,
    IQuestionService questionService) : BaseController
    {
        private readonly IQuizService _quizService = quizService;
        private readonly IQuestionService _questionService = questionService;

        public async Task<IActionResult> Index()
        {
            var instructorQuestions = (await _questionService.GetQuestionsByInstructorAsync(GetUserId())).Data;
            return View(instructorQuestions);
        }

        [HttpGet("ManageQuestions/{quizId}")]
        public async Task<IActionResult> ManageQuestions(int quizId)
        {
            var quiz = (await _quizService.GetQuizByIdAsync(quizId)).Data;

            var quizQuestions = (await _questionService.GetQuestionsByQuizIdAsync(quizId)).Data;
            ViewBag.QuizTitle = quiz.Title;
            ViewBag.QuizId = quiz.Id;
            return View(quizQuestions);
        }

        [HttpGet("CreateQuestion/{quizId}")]
        public async Task<IActionResult> CreateQuestion(int quizId)
        {
            var quiz = (await _quizService.GetQuizByIdAsync(quizId)).Data;

            QuestionDTO questionDTO = new()
            {
                QuizDto = quiz
            };

            return View(questionDTO);
        }

        [HttpPost("CreateQuestion/{quizId}")]
        public async Task<IActionResult> CreateQuestion(QuestionDTO questionDTO)
        {
            var result = await _questionService.CreateQuestionAsync(questionDTO);

            if (result.Success)
            {
                TempData["success"] = "Question created successfully!";
                return RedirectToAction(nameof(ManageQuestions), new { quizId = questionDTO.QuizId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(questionDTO);
        }

        [HttpGet("EditQuestion/{questionId}")]
        public async Task<IActionResult> EditQuestion(int questionId)
        {
            var question = (await _questionService.GetQuestionByIdAsync(questionId)).Data;
            return View(question);
        }

        [HttpPost("EditQuestion/{questionId}")]
        public async Task<IActionResult> EditQuestion(QuestionDTO questionDTO)
        {
            var result = await _questionService.UpdateQuestionAsync(questionDTO);

            if (result.Success)
            {
                TempData["success"] = "Question updated successfully!";
                return RedirectToAction(nameof(ManageQuestions), new { quizId = questionDTO.QuizId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(questionDTO);
        }

        [HttpGet("DeleteQuestion/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var question = (await _questionService.GetQuestionByIdAsync(questionId)).Data;
            return View(question);
        }

        [HttpPost("DeleteQuestion/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(QuestionDTO questionDTO)
        {
            var result = await _questionService.DeleteQuestionAsync(questionDTO);

            if (result.Success)
            {
                TempData["success"] = "Question deleted successfully!";
                return RedirectToAction(nameof(ManageQuestions), new { quizId = questionDTO.QuizId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(questionDTO);
        }
    }
}
