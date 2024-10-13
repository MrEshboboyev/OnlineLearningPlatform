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
    [Route($"{SD.Role_Instructor}/Module")]
    public class ModuleController(ICourseService courseService,
        IModuleService moduleService,
        ILessonService lessonService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;
        private readonly IModuleService _moduleService = moduleService;
        private readonly ILessonService _lessonService = lessonService;

        public async Task<IActionResult> Index()
        {
            var instructorModules = (await _moduleService.GetModulesByInstructorAsync(GetUserId())).Data;
            return View(instructorModules);
        }

        [HttpGet("ManageModules/{courseId}")]
        public async Task<IActionResult> ManageModules(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;

            var courseModules = (await _courseService.GetModulesByCourseAsync(courseId)).Data;

            ViewBag.CourseId = course.Id;
            ViewBag.CourseTitle = course.Title;
            return View(courseModules);
        }

        [HttpGet("CreateModule/{courseId}")]
        public async Task<IActionResult> CreateModule(int courseId)
        {
            var course = (await _courseService.GetCourseByIdAsync(courseId)).Data;

            ModuleDTO moduleDTO = new()
            {
                CourseDTO = course
            };

            return View(moduleDTO);
        }

        [HttpPost("CreateModule/{courseId}")]
        public async Task<IActionResult> CreateModule(ModuleDTO moduleDTO)
        {
            var result = await _courseService.AddModuleToCourseAsync(moduleDTO.CourseId, moduleDTO);

            if (result.Success)
            {
                TempData["success"] = "Module added successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(moduleDTO);
        }

        [HttpGet("EditModule/{moduleId}")]
        public async Task<IActionResult> EditModule(int moduleId)
        {
            var module = (await _moduleService.GetModuleByIdAsync(moduleId)).Data;
            return View(module);
        }

        [HttpPost("EditModule/{moduleId}")]
        public async Task<IActionResult> EditModule(ModuleDTO moduleDTO)
        {
            var result = await _moduleService.UpdateModuleAsync(moduleDTO);

            if (result.Success)
            {
                TempData["success"] = "Module updated successfully!";
                return RedirectToAction(nameof(ManageModules), new { courseId = moduleDTO.CourseId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(moduleDTO);
        }

        [HttpGet("DeleteModule/{moduleId}")]
        public async Task<IActionResult> DeleteModule(int moduleId)
        {
            var module = (await _moduleService.GetModuleByIdAsync(moduleId)).Data;
            return View(module);
        }

        [HttpPost("DeleteModule/{moduleId}")]
        public async Task<IActionResult> DeleteModule(ModuleDTO moduleDTO)
        {
            var result = await _moduleService.DeleteModuleAsync(moduleDTO);

            if (result.Success)
            {
                TempData["success"] = "Module deleted successfully!";
                return RedirectToAction(nameof(ManageModules), new { courseId = moduleDTO.CourseId });
            }

            TempData["error"] = $"Error : {result.Message}";
            return View(moduleDTO);
        }
    }
}
