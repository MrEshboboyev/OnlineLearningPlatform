using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.UI.Controllers;

namespace OnlineLearningPlatform.UI.Areas.Instructor.Controllers
{
    [Area(SD.Role_Instructor)]
    [Authorize(Roles = SD.Role_Instructor)]
    public class CourseController(ICourseService courseService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;

        public async Task<IActionResult> Index()
        {
            var allCourses = (await _courseService.GetCoursesByInstructorAsync(GetUserId())).Data;
            return View(allCourses);
        }
    }
}
