﻿using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class ProgressService(IUnitOfWork unitOfWork, IMapper mapper) : IProgressService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region Progress Tracking
    public async Task<ResponseDTO<ProgressDTO>> GetStudentProgressInLessonAsync(string studentId, int lessonId)
    {
        try
        {
            var studentProgressForLesson = await _unitOfWork.Progress.GetAsync(
                p => p.StudentId.Equals(studentId) && p.LessonId.Equals(lessonId), 
                includeProperties: "Student,Lesson");

            var mappedProgress = _mapper.Map<ProgressDTO>(studentProgressForLesson);

            return new ResponseDTO<ProgressDTO>(mappedProgress);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<ProgressDTO>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<ProgressDTO>>> GetStudentProgressInCourseAsync(string studentId, int courseId)
    {
        try
        {
            IList<ProgressDTO> studentProgressesInCourse = [];

            var courseLessons = await _unitOfWork.Lesson.GetAllAsync(
                filter: l => l.Module.CourseId.Equals(courseId));

            // each progress for this course
            foreach (var lesson in courseLessons)
            {
                // get progress (lesson and student)
                var progress = await _unitOfWork.Progress.GetAsync(
                    p => p.LessonId.Equals(lesson.Id),
                    includeProperties: "Student,Lesson.Module.Course");

                studentProgressesInCourse.Add(_mapper.Map<ProgressDTO>(progress));
            }

            return new ResponseDTO<IEnumerable<ProgressDTO>>(studentProgressesInCourse);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<ProgressDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<ProgressDTO>>> GetProgressesInCourseAsync(int courseId)
    {
        try
        {
            var courseProgresses = await _unitOfWork.Progress.GetAllAsync(
                filter: p => p.Lesson.Module.CourseId.Equals(courseId),
                includeProperties: "Student,Lesson.Module.Course");

            var mappedProgresses = _mapper.Map<IEnumerable<ProgressDTO>>(courseProgresses);

            return new ResponseDTO<IEnumerable<ProgressDTO>>(mappedProgresses);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<ProgressDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<double>> GetOverallProgressInCourseAsync(string studentId, int courseId)
    {
        try
        {
            // Step 1: Retrieve all lessons for the given course.
            var lessons = await _unitOfWork.Lesson.GetAllAsync(
                l => l.Module.CourseId.Equals(courseId));
            if (lessons == null || !lessons.Any())
            {
                return new ResponseDTO<double>("No lessons found for the specified course.");
            }

            // Step 2: Get the progress records for the student for the lessons in the course.
            var lessonIds = lessons.Select(l => l.Id).ToList();
            var progressList = await _unitOfWork.Progress.GetAllAsync(
                p => p.StudentId.Equals(studentId) && 
                lessonIds.Contains(p.LessonId));

            // Step 3: Calculate the number of completed lessons by the student.
            int completedLessonsCount = progressList.Count(p => p.IsCompleted);
            int totalLessonsCount = lessons.Count();

            // Step 4: Calculate the overall progress as a percentage.
            double progressPercentage = (double)completedLessonsCount / totalLessonsCount * 100;

            // Return the result.
            return new ResponseDTO<double>(progressPercentage);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }
    #endregion

    #region Marking Progress
    // POST actions
    public async Task<ResponseDTO<object>> MarkLessonAsCompletedAsync(string studentId, int lessonId)
    {
        try
        {
            // lesson is complete for student, set to relation progress status is Completed;
            var progressFromDb = await _unitOfWork.Progress.GetAsync(
                p => p.LessonId.Equals(lessonId) && p.StudentId.Equals(studentId)
                ) ?? throw new Exception("Progress not found!");

            progressFromDb.IsCompleted = true;

            await _unitOfWork.Progress.UpdateAsync(progressFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UnmarkLessonAsCompletedAsync(string studentId, int lessonId)
    {
        try
        {
            // lesson is complete for student unmark, set to relation progress status is Completed to false;
            var progressFromDb = await _unitOfWork.Progress.GetAsync(
                p => p.LessonId.Equals(lessonId) && p.StudentId.Equals(studentId)
                ) ?? throw new Exception("Progress not found!");

            progressFromDb.IsCompleted = false;

            await _unitOfWork.Progress.UpdateAsync(progressFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UpdateCompletionDateAsync(string studentId, int lessonId, DateTime completionDate)
    {
        try
        {
            var progressFromDb = await _unitOfWork.Progress.GetAsync(
                p => p.LessonId.Equals(lessonId) && p.StudentId.Equals(studentId)
                ) ?? throw new Exception("Progress not found!");

            progressFromDb.CompletionDate = completionDate;

            await _unitOfWork.Progress.UpdateAsync(progressFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Progress Validation
    public async Task<ResponseDTO<bool>> IsLessonCompletedAsync(string studentId, int lessonId)
    {
        try
        {
            // lesson is complete for student, set to relation progress status is Completed;
            var progressFromDb = await _unitOfWork.Progress.GetAsync(
                p => p.LessonId.Equals(lessonId) && p.StudentId.Equals(studentId)
                ) ?? throw new Exception("Progress not found!");

            return new ResponseDTO<bool>(progressFromDb.IsCompleted);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    public async Task<ResponseDTO<DateTime?>> GetLessonCompletionDateAsync(string studentId, int lessonId)
    {
        try
        {
            var progressFromDb = await _unitOfWork.Progress.GetAsync(
                p => p.LessonId.Equals(lessonId) && p.StudentId.Equals(studentId)
                ) ?? throw new Exception("Progress not found!");

            return new ResponseDTO<DateTime?>(progressFromDb.CompletionDate);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<DateTime?>(ex.Message);
        }
    }
    #endregion

    #region Progress History
    public async Task<ResponseDTO<IEnumerable<ProgressDTO>>> GetProgressHistoryForStudentAsync(string studentId)
    {
        try
        {
            var studentProgresses = await _unitOfWork.Progress.GetAllAsync(
                filter: p => p.StudentId.Equals(studentId),
                includeProperties: "Student,Lesson.Module.Course");

            // mapping
            var mappedProgresses = _mapper.Map<IEnumerable<ProgressDTO>>(studentProgresses);

            return new ResponseDTO<IEnumerable<ProgressDTO>>(mappedProgresses);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<ProgressDTO>>(ex.Message);
        }
    }
    #endregion

    #region Course Completion
    public async Task<ResponseDTO<bool>> IsCourseFullyCompletedAsync(string studentId, int courseId)
    {
        try
        {
            IList<ProgressDTO> studentProgressesInCourse = [];

            var courseLessons = await _unitOfWork.Lesson.GetAllAsync(
                filter: l => l.Module.CourseId.Equals(courseId));

            // each progress for this course
            foreach (var lesson in courseLessons)
            {
                // get progress (lesson and student)
                var progress = await _unitOfWork.Progress.GetAsync(
                    p => p.LessonId.Equals(lesson.Id),
                    includeProperties: "Student,Lesson");

                studentProgressesInCourse.Add(_mapper.Map<ProgressDTO>(progress));
            }

            return new ResponseDTO<bool>(studentProgressesInCourse.Any(p => !p.IsCompleted));
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    public async Task<ResponseDTO<DateTime?>> GetCourseCompletionDateAsync(string studentId, int courseId)
    {
        try
        {
            IList<ProgressDTO> studentProgressesInCourse = [];

            var courseLessons = await _unitOfWork.Lesson.GetAllAsync(
                filter: l => l.Module.CourseId.Equals(courseId));

            // each progress for this course
            foreach (var lesson in courseLessons)
            {
                // get progress (lesson and student)
                var progress = await _unitOfWork.Progress.GetAsync(
                    p => p.LessonId.Equals(lesson.Id),
                    includeProperties: "Student,Lesson");

                studentProgressesInCourse.Add(_mapper.Map<ProgressDTO>(progress));
            }

            return new ResponseDTO<DateTime?>(studentProgressesInCourse.Max(p => p.CompletionDate));
        }
        catch (Exception ex)
        {
            return new ResponseDTO<DateTime?>(ex.Message);
        }
    }
    #endregion

    #region Progress Statistics
    public async Task<ResponseDTO<int>> GetCompletedLessonsCountAsync(string studentId, int courseId)
    {
        try
        {
            IList<ProgressDTO> studentProgressesInCourse = [];

            var courseLessons = await _unitOfWork.Lesson.GetAllAsync(
                filter: l => l.Module.CourseId.Equals(courseId));

            // each progress for this course
            foreach (var lesson in courseLessons)
            {
                // get progress (lesson and student)
                var progress = await _unitOfWork.Progress.GetAsync(
                    p => p.LessonId.Equals(lesson.Id),
                    includeProperties: "Student,Lesson");

                studentProgressesInCourse.Add(_mapper.Map<ProgressDTO>(progress));
            }

            return new ResponseDTO<int>(studentProgressesInCourse.Count(p => p.IsCompleted));
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }
    public async Task<ResponseDTO<Dictionary<string, double>>> GetCourseCompletionStatisticsAsync()
    {
        try
        {
            // Step 1: Retrieve all courses from the repository.
            var courses = await _unitOfWork.Course.GetAllAsync();
            if (courses == null || !courses.Any())
            {
                return new ResponseDTO<Dictionary<string, double>>("No courses found.");
            }

            var courseCompletionStats = new Dictionary<string, double>();

            // Step 2: Loop through each course to calculate its completion rate.
            foreach (var course in courses)
            {
                // Step 3: Retrieve all lessons for the current course.
                var lessons = await _unitOfWork.Lesson.GetAllAsync(
                    l => l.Module.CourseId.Equals(course.Id));
                if (lessons == null || !lessons.Any())
                {
                    continue; // Skip the course if it has no lessons.
                }

                var lessonIds = lessons.Select(l => l.Id).ToList();

                // Step 4: Retrieve progress for all students for the lessons in this course.
                var progressList = await _unitOfWork.Progress.GetAllAsync(p => lessonIds.Contains(p.LessonId));

                // Step 5: Calculate the total number of lesson completions and overall lessons count.
                int totalLessonsCount = lessons.Count();
                int totalStudents = progressList.Select(p => p.StudentId).Distinct().Count();
                int totalCompletions = progressList.Count(p => p.IsCompleted);

                // If there are no students or lessons, skip the calculation for this course.
                if (totalStudents == 0 || totalLessonsCount == 0)
                {
                    continue;
                }

                // Step 6: Calculate the course completion rate as a percentage.
                double courseCompletionPercentage = (double)totalCompletions / (totalStudents * totalLessonsCount) * 100;

                // Add the course completion percentage to the result dictionary.
                courseCompletionStats.Add(course.Title, courseCompletionPercentage);
            }

            // Return the course completion statistics.
            return new ResponseDTO<Dictionary<string, double>>(courseCompletionStats);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<Dictionary<string, double>>(ex.Message);
        }
    }
    #endregion

    public async Task UpdateProgressForNewLesson(int moduleId, Lesson lesson)
    {
        // Retrieve the module along with its course and enrolled users
        var module = await _unitOfWork.Module.GetAsync(
            m => m.Id == moduleId,
            includeProperties: "Course.Enrollments") 
            ?? throw new Exception("Module not found!");

        // Iterate through each enrolled user in the course
        foreach (var enrollment in module.Course.Enrollments)
        {
            var userId = enrollment.StudentId;

            // Create a new progress entity for the new lesson and the enrolled user
            var progress = new Progress
            {
                LessonId = lesson.Id,
                StudentId = userId
            };

            // Add the new progress entry for this user
            await _unitOfWork.Progress.AddAsync(progress);
        }

        // Save the progress records
        await _unitOfWork.SaveAsync();
    }

}

