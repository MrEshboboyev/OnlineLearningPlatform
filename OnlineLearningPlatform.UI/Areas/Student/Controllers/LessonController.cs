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
    public class LessonController(ILessonService lessonService,
        IProgressService progressService) : BaseController
    {
        private readonly ILessonService _lessonService = lessonService;
        private readonly IProgressService _progressService = progressService;

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

            // IsCompleted
            ViewBag.IsCompleted = (await _lessonService.IsLessonCompletedAsync(lessonId, GetUserId())).Data;
            return View(lesson);
        }

        [HttpPost("MarkLessonComplete")]
        public async Task<IActionResult> ToggleLessonCompletion(int lessonId)
        {
            var isCompleted = (await _lessonService.IsLessonCompletedAsync(lessonId, GetUserId())).Data;

            if (isCompleted)
            {
                await _progressService.UnmarkLessonAsCompletedAsync(GetUserId(), lessonId);
                TempData["success"] = "Lesson is uncompleted!";
            }
            else
            {
                await _progressService.MarkLessonAsCompletedAsync(GetUserId(), lessonId);
                TempData["success"] = "Lesson is completed!";
            }

            return RedirectToAction(nameof(Details), new { lessonId });
        }
    }
}
