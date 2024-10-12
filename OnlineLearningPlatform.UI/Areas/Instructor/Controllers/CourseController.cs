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
    public class CourseController(ICourseService courseService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;

        public async Task<IActionResult> Index()
        {
            var allCourses = (await _courseService.GetCoursesByInstructorAsync(GetUserId())).Data;
            return View(allCourses);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;
            return View(course);
        }
        
        [HttpGet]
        public IActionResult Create() => View(new CourseDTO());

        [HttpPost]
        public async Task<IActionResult> Create(CourseDTO courseDTO)
        {
            // assign instructorId
            courseDTO.InstructorId = GetUserId();

            var result = await _courseService.CreateCourseAsync(courseDTO);

            if (result.Success)
            {
                TempData["success"] = "Course created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(courseDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseDTO courseDTO)
        {
            // assign instructorId
            courseDTO.InstructorId = GetUserId();

            var result = await _courseService.UpdateCourseAsync(courseDTO);

            if (result.Success)
            {
                TempData["success"] = "Course updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(courseDTO);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CourseDTO courseDTO)
        {
            // assign instructorId
            courseDTO.InstructorId = GetUserId();

            var result = await _courseService.DeleteCourseAsync(courseDTO);

            if (result.Success)
            {
                TempData["success"] = "Course deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(courseDTO);
        }
    }
}
