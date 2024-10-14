using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.UI.Controllers;

namespace OnlineLearningPlatform.UI.Areas.Student.Controllers
{
    [Area(SD.Role_Student)]
    [Authorize(Roles = SD.Role_Student)]
    [Route($"{SD.Role_Student}/Course")]
    public class CourseController(ICourseService courseService, 
        IEnrollmentService enrollmentService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;
        private readonly IEnrollmentService _enrollmentService = enrollmentService;

        public async Task<IActionResult> Index()
        {
            var allCourses = (await _courseService.GetAllCoursesAsync()).Data;
            return View(allCourses);
        }

        [HttpGet("Details/{courseId}")]
        public async Task<IActionResult> Details(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;
            return View(course);    
        }

        [HttpPost("Enroll/{courseId}")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var result = await _courseService.EnrollUserInCourseAsync(courseId, GetUserId());

            if (result.Success)
            {
                TempData["success"] = result.Message;
                return RedirectToAction(nameof(EnrolledCourses));
            }

            TempData["error"] = $"Error : {result.Message}";
            return RedirectToAction(nameof(Details), new { courseId });
        }

        [HttpPost("Unenroll/{courseId}")]
        public async Task<IActionResult> Unenroll(int courseId)
        {
            var result = await _courseService.UnenrollUserFromCourseAsync(courseId, GetUserId());

            if (result.Success)
            {
                TempData["success"] = result.Message;
                return RedirectToAction(nameof(EnrolledCourses));
            }

            TempData["error"] = $"Error : {result.Message}";
            return RedirectToAction(nameof(Details), new { courseId });
        }

        [HttpGet("EnrolledCourses")]
        public async Task<IActionResult> EnrolledCourses()
        {
            var enrolledCourses = (await _enrollmentService.GetEnrollmentsByStudentAsync(GetUserId())).Data;
            return View(enrolledCourses);
        }
    }
}
