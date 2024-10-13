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
    [Route($"{SD.Role_Instructor}/Lesson")]
    public class LessonController(IModuleService moduleService,
        ILessonService lessonService) : BaseController
    {
        private readonly IModuleService _moduleService = moduleService;
        private readonly ILessonService _lessonService = lessonService;

        public async Task<IActionResult> Index()
        {
            var instructorLessons = (await _lessonService.GetLessonsByInstructorAsync(GetUserId())).Data;
            return View(instructorLessons);
        }

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
    }
}
