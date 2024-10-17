using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.UI.Controllers;

namespace OnlineLearningPlatform.UI.Areas.Student.Controllers
{
    [Area(SD.Role_Student)]
    [Authorize(Roles = SD.Role_Student)]
    [Route($"{SD.Role_Student}/Lesson")]
    public class LessonController(ILessonService lessonService) : BaseController
    {
        private readonly ILessonService _lessonService = lessonService;

        [HttpGet("{courseId}")]
        public async Task<IActionResult> Index(int courseId)
        {
            var courseLessons = (await _lessonService.GetLessonsByCourseAsync(courseId)).Data;
            return View(courseLessons);
        }

        [HttpGet("Details/{lessonId}")]
        public async Task<IActionResult> Details(int lessonId)
        {
            var lesson = (await _lessonService.GetLessonByIdAsync(lessonId)).Data;
            return View(lesson);
        }

        [HttpPost("MarkLessonComplete")]
        public async Task<IActionResult> MarkLessonComplete(int lessonId)
        {
            var result = await _lessonService.UpdateStudentProgressInLessonAsync(lessonId, GetUserId());

            if (result.Success)
            {
                TempData["success"] = "Lesson is completed!";
            }
            else
            {
                TempData["error"] = $"Error : {result.Message}";
            }

            return RedirectToAction(nameof(Details), lessonId);
        }
    }
}
