using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;

namespace OnlineLearningPlatform.UI.Controllers
{
    public class DashboardController(ICourseService courseService,
        IProgressService progressService,
        IEnrollmentService enrollmentService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;
        private readonly IProgressService _progressService = progressService;
        private readonly IEnrollmentService _enrollmentService = enrollmentService;

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = SD.Role_Instructor)]
        public async Task<IActionResult> Instructor()
        {
            var courses = (await _courseService.GetCoursesByInstructorAsync(GetUserId())).Data; // Assume this method gets courses with stats

            List<CourseStatisticsDTO> courseStatistics = [];

            foreach (var course in courses)
            {
                CourseStatisticsDTO courseStatisticsDTO = new()
                {
                    EnrollmentCount = course.Enrollments.Count,
                    CourseTitle = course.Title,
                    AverageCompletionPercentage = course.Enrollments.Average(e => e.ProgressPercentage)
                };

                courseStatistics.Add(courseStatisticsDTO);
            }

            return View(courseStatistics);
        }

        [Authorize(Roles = SD.Role_Student)]
        public async Task<IActionResult> Student()
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
