using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.UI.Controllers;

namespace OnlineLearningPlatform.UI.Areas.Student.Controllers
{
    [Area(SD.Role_Student)]
    [Authorize(Roles = SD.Role_Student)]
    [Route($"{SD.Role_Student}/Progress")]
    public class ProgressController(IProgressService progressService) : BaseController
    {
        private readonly IProgressService _progressService = progressService;

        public async Task<IActionResult> Index()
        {
            var studentProgresses = (await _progressService.GetProgressHistoryForStudentAsync(GetUserId())).Data;
            return View(studentProgresses);
        }

        [HttpGet("CourseProgress/{courseId}")]
        public async Task<IActionResult> CourseProgress(int courseId)
        {
            var courseProgress = (await _progressService.GetStudentProgressInCourseAsync(GetUserId(), courseId)).Data;
            return View(courseProgress);
        }
    }
}
