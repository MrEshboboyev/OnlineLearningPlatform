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
    [Route($"{SD.Role_Instructor}/Progress")]
    public class ProgressController(IProgressService progressService) : BaseController
    {
        private readonly IProgressService _progressService = progressService;

        // Index action to show general course progress with a list of students
        [HttpGet("Student/{courseId}")]
        public async Task<IActionResult> Index(int courseId)
        {
            var courseProgresses = (await _progressService.GetProgressesInCourseAsync(courseId)).Data;

            // Group by student and calculate progress
            var studentProgress = courseProgresses
                .GroupBy(p => new { p.StudentId, p.StudentDTO.UserName })
                .Select(g => new StudentProgressDTO
                {
                    StudentId = g.Key.StudentId,
                    UserName = g.Key.UserName,
                    TotalLessons = g.Count(),
                    CompletedLessons = g.Count(p => p.IsCompleted),
                    CourseId = courseId
                })
                .ToList();

            return View(studentProgress);
        }


        // View the progress of an individual student
        [HttpGet("StudentDetails/{courseId}/{studentId}")]
        public async Task<IActionResult> StudentDetails(int courseId, string studentId)
        {
            // Fetch detailed progress for this student in the course
            var studentProgressInCourse = (await _progressService.GetStudentProgressInCourseAsync(studentId, courseId)).Data;

            // Send the progress data to the view
            return View(studentProgressInCourse);
        }
    }
}
