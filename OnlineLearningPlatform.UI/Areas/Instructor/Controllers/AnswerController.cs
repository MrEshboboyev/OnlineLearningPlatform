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
    [Route($"{SD.Role_Instructor}/Answer")]
    public class AnswerController(IQuestionService questionService,
        IAnswerService answerService) : BaseController
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IAnswerService _answerService = answerService;

        [HttpGet("ManageAnswers/{questionId}")]
        public async Task<IActionResult> ManageAnswers(int questionId)
        {
            var question = (await _questionService.GetQuestionByIdAsync(questionId)).Data;

            var questionAnswers = (await _answerService.GetAnswersByQuestionIdAsync(questionId)).Data;

            ViewBag.QuestionId = question.Id;
            ViewBag.QuestionText = question.QuestionText;
            return View(questionAnswers);
        }

        [HttpGet("CreateAnswer/{questionId}")]
        public async Task<IActionResult> CreateAnswer(int questionId)
        {
            var question = (await _questionService.GetQuestionByIdAsync(questionId)).Data;

            AnswerDTO answerDTO = new()
            {
                QuestionDTO = question
            };

            return View(answerDTO);
        }

        [HttpPost("CreateAnswer/{questionId}")]
        public async Task<IActionResult> CreateAnswer(int questionId, AnswerDTO answerDTO)
        {
            var result = await _answerService.AddAnswerToQuestionAsync(questionId, answerDTO);

            if (result.Success)
            {
                TempData["success"] = "Answer added successfully!";
                return RedirectToAction(nameof(ManageAnswers), new { questionId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(answerDTO);
        }

        [HttpGet("EditAnswer/{answerId}")]
        public async Task<IActionResult> EditAnswer(int answerId)
        {
            var answer = (await _answerService.GetAnswerByIdAsync(answerId)).Data;
            return View(answer);
        }

        [HttpPost("EditAnswer/{answerId}")]
        public async Task<IActionResult> EditAnswer(AnswerDTO answerDTO)
        {
            var result = await _answerService.UpdateAnswerAsync(answerDTO);

            if (result.Success)
            {
                TempData["success"] = "Answer updated successfully!";
                return RedirectToAction(nameof(ManageAnswers), new { questionId = answerDTO.QuestionId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(answerDTO);
        }

        [HttpGet("DeleteAnswer/{answerId}")]
        public async Task<IActionResult> DeleteAnswer(int answerId)
        {
            var answer = (await _answerService.GetAnswerByIdAsync(answerId)).Data;
            return View(answer);
        }

        [HttpPost("DeleteAnswer/{answerId}")]
        public async Task<IActionResult> DeleteAnswer(AnswerDTO answerDTO)
        {
            var result = await _answerService.DeleteAnswerFromQuestionAsync(answerDTO.Id);

            if (result.Success)
            {
                TempData["success"] = "Answer deleted successfully!";
                return RedirectToAction(nameof(ManageAnswers), new { questionId = answerDTO.QuestionId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(answerDTO);
        }
    }
}
