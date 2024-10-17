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
    [Route($"{SD.Role_Student}/Dashboard")]
    public class DashboardController(ICourseService courseService,
        IEnrollmentService enrollmentService,
        IProgressService progressService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;
        private readonly IEnrollmentService _enrollmentService = enrollmentService;
        private readonly IProgressService _progressService = progressService;

        public async Task<IActionResult> Index()
        {
            var enrollments = (await _enrollmentService.GetEnrollmentsByStudentAsync(GetUserId())).Data; // Assume this method gets student progress

            List<StudentCourseProgressDTO> studentCourseProgress = [];

            foreach (var enrollment in enrollments)
            {
                var course = (await _courseService.GetCourseByIdAsync(enrollment.CourseId)).Data;

                StudentCourseProgressDTO studentCourseProgressDTO = new()
                {
                    CourseTitle = course.Title,
                    CompletedLessons = (await _progressService.GetCompletedLessonsCountAsync(GetUserId(),
                    course.Id)).Data,
                    TotalLessons = (await _courseService.GetLessonsCountByCourseAsync(course.Id)).Data
                };

                studentCourseProgress.Add(studentCourseProgressDTO);
            }

            return View(studentCourseProgress);
        }
    }
}
