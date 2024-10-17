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
    [Route($"{SD.Role_Instructor}/Dashboard")]
    public class DashboardController(ICourseService courseService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;

        public async Task<IActionResult> Index()
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
    }
}
