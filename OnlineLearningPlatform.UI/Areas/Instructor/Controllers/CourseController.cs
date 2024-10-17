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
    [Route($"{SD.Role_Instructor}/Course")]
    public class CourseController(ICourseService courseService,
        IModuleService moduleService,
        ILessonService lessonService,
        IProgressService progressService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;
        private readonly IModuleService _moduleService = moduleService;
        private readonly ILessonService _lessonService = lessonService;
        private readonly IProgressService _progressService = progressService;

        #region Course Management
        public async Task<IActionResult> Index()
        {
            var allCourses = (await _courseService.GetCoursesByInstructorAsync(GetUserId())).Data;
            return View(allCourses);
        }

        [HttpGet("Details/{courseId}")]
        public async Task<IActionResult> Details(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;
            return View(course);
        }

        [HttpGet("Create")]
        public IActionResult Create() => View(new CourseDTO());

        [HttpPost("Create")]
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

        [HttpGet("Update/{courseId}")]
        public async Task<IActionResult> Update(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;
            return View(course);
        }

        [HttpPost("Update/{courseId}")]
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

        [HttpGet("Delete/{courseId}")]
        public async Task<IActionResult> Delete(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;
            return View(course);
        }

        [HttpPost("Delete/{courseId}")]
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
        #endregion

        #region Module-Lesson management
        [HttpGet("ManageLessons/{moduleId}")]
        public async Task<IActionResult> ManageLessons(int moduleId)
        {
            var module = (await _moduleService.GetModuleByIdAsync(moduleId)).Data;

            var moduleLessons = (await _moduleService.GetLessonsByModuleAsync(moduleId)).Data;

            ViewBag.ModuleId = module.Id;
            ViewBag.ModuleTitle = module.Title;
            return View(moduleLessons);
        }

        [HttpGet("CreateLesson/{moduleId}")]
        public async Task<IActionResult> CreateLesson(int moduleId)
        {
            var module = (await _moduleService.GetModuleByIdAsync(moduleId)).Data;

            LessonDTO lessonDTO = new()
            {
                ModuleDTO = module
            };

            return View(lessonDTO);
        }

        [HttpPost("CreateLesson/{moduleId}")]
        public async Task<IActionResult> CreateLesson(int moduleId, LessonDTO lessonDTO)
        {
            var result = await _moduleService.AddLessonToModuleAsync(moduleId, lessonDTO);

            if (result.Success)
            {
                TempData["success"] = "Lesson added successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(lessonDTO);
        }

        [HttpGet("EditLesson/{lessonId}")]
        public async Task<IActionResult> EditLesson(int lessonId)
        {
            var module = (await _lessonService.GetLessonByIdAsync(lessonId)).Data;
            return View(module);
        }

        [HttpPost("EditLesson/{lessonId}")]
        public async Task<IActionResult> EditLesson(LessonDTO lessonDTO)
        {
            var result = await _lessonService.UpdateLessonAsync(lessonDTO);

            if (result.Success)
            {
                TempData["success"] = "Lesson updated successfully!";
                return RedirectToAction(nameof(ManageLessons), new { moduleId = lessonDTO.ModuleId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(lessonDTO);
        }
        #endregion

        #region Course-Enrollment management
        [HttpGet("Enrollments/{courseId}")]
        public async Task<IActionResult> Enrollments(int courseId)
        {
            var courseEnrollments = (await _courseService.GetEnrollmentsByCourseAsync(courseId)).Data;
            return View(courseEnrollments);
        }

        [HttpPost("RemoveEnrollment/{courseId}/{studentId}")]
        public async Task<IActionResult> RemoveEnrollment(int courseId, string studentId)
        {
            var result = await _courseService.UnenrollUserFromCourseAsync(courseId, studentId);

            if (result.Success)
            {
                TempData["success"] = "Student unenrolled from course successfully!";
            }
            else
            {
                TempData["error"] = $"Error : {result.Message}";
            }

            return RedirectToAction(nameof(Enrollments), new { courseId });
        }
        #endregion
    }
}
