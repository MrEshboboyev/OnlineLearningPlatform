using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.UI.Controllers;

namespace OnlineLearningPlatform.UI.Areas.Instructor.Controllers
{
    [Area(SD.Role_Instructor)]
    [Authorize(Roles = SD.Role_Instructor)]
    [Route($"{SD.Role_Instructor}/Progress")]
    public class ProgressController(ILessonService lessonService,
        IProgressService progressService) : BaseController
    {
        private readonly ILessonService _lessonService = lessonService;
        private readonly IProgressService _progressService = progressService;

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("Student/{courseId}")]
        public async Task<IActionResult> Student(int courseId)
        {
            var courseProgresses = (await _progressService.GetProgressesInCourseAsync(courseId)).Data;
            return View(courseProgresses);
        }

        [HttpGet("StudentDetails/{courseId}/{studentId}")]
        public async Task<IActionResult> StudentDetails(int courseId, string studentId)
        {
            var studentProgressInCourse = (await _progressService.
                GetStudentProgressInCourseAsync(studentId, courseId)).Data;
            return View(studentProgressInCourse);
        }
    }
}
