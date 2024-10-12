using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.Services;

namespace OnlineLearningPlatform.UI.Areas.Student.Controllers
{
    [Area(SD.Role_Student)]
    [Authorize(Roles = SD.Role_Student)]
    [Route($"{SD.Role_Student}/Course")]
    public class CourseController(ICourseService courseService) : Controller
    {
        private readonly ICourseService _courseService = courseService;

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
    }
}
