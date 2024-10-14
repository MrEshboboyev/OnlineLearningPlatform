using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class LessonService(IUnitOfWork unitOfWork, IMapper mapper) : ILessonService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region Lesson Management
    public async Task<ResponseDTO<IEnumerable<LessonDTO>>> GetAllLessonsAsync()
    {
        try
        {
            var allLessons = await _unitOfWork.Lesson.GetAllAsync(
                includeProperties: "Module");

            var mappedLessons = _mapper.Map<IEnumerable<LessonDTO>>(allLessons);

            return new ResponseDTO<IEnumerable<LessonDTO>>(mappedLessons);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<LessonDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<LessonDTO>>> GetLessonsByModuleAsync(int moduleId)
    {
        try
        {
            var moduleLessons = await _unitOfWork.Lesson.GetAllAsync(
                filter: l => l.ModuleId.Equals(moduleId),
                includeProperties: "Module");

            var mappedLessons = _mapper.Map<IEnumerable<LessonDTO>>(moduleLessons);

            return new ResponseDTO<IEnumerable<LessonDTO>>(mappedLessons);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<LessonDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<LessonDTO>>> GetLessonsByInstructorAsync(string instructorId)
    {
        try
        {
            var instructorLessons = await _unitOfWork.Lesson.GetAllAsync(
                filter: l => l.Module.Course.InstructorId.Equals(instructorId),
                includeProperties: "Module.Course");

            var mappedLessons = _mapper.Map<IEnumerable<LessonDTO>>(instructorLessons);

            return new ResponseDTO<IEnumerable<LessonDTO>>(mappedLessons);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<LessonDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<LessonDTO>>> GetLessonsByCourseAsync(int courseId)
    {
        try
        {
            var courseLessons = await _unitOfWork.Lesson.GetAllAsync(
                filter: l => l.Module.CourseId.Equals(courseId),
                includeProperties: "Module.Course");

            var mappedLessons = _mapper.Map<IEnumerable<LessonDTO>>(courseLessons);

            return new ResponseDTO<IEnumerable<LessonDTO>>(mappedLessons);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<LessonDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<LessonDTO>> GetLessonByIdAsync(int lessonId)
    {
        try
        {
            var lesson = await _unitOfWork.Lesson.GetAsync(
                filter: l => l.Id.Equals(lessonId),
                includeProperties: "Module.Course");

            var mappedLesson = _mapper.Map<LessonDTO>(lesson);

            return new ResponseDTO<LessonDTO>(mappedLesson);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<LessonDTO>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> CreateLessonAsync(LessonDTO lessonDTO)
    {
        try
        {
            var lessonForDb = _mapper.Map<Lesson>(lessonDTO);

            await _unitOfWork.Lesson.AddAsync(lessonForDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Create lesson!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UpdateLessonAsync(LessonDTO lessonDTO)
    {
        try
        {
            var lessonFromDb = await _unitOfWork.Lesson.GetAsync(
                l => l.Id.Equals(lessonDTO.Id)
                ) ?? throw new Exception("Lesson not found!");

            // mapping fields
            _mapper.Map(lessonDTO, lessonFromDb);

            await _unitOfWork.Lesson.UpdateAsync(lessonFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Update lesson!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> DeleteLessonAsync(LessonDTO lessonDTO)
    {
        try
        {
            var lessonFromDb = await _unitOfWork.Lesson.GetAsync(
                l => l.Id.Equals(lessonDTO.Id)
                ) ?? throw new Exception("Lesson not found!");

            await _unitOfWork.Lesson.RemoveAsync(lessonFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Delete lesson!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Lesson Content Management
    public async Task<ResponseDTO<string>> GetLessonContentAsync(int lessonId)
    {
        try
        {
            var lessonFromDb = await _unitOfWork.Lesson.GetAsync(
                l => l.Id.Equals(lessonId)
                ) ?? throw new Exception("Lesson not found");

            return new ResponseDTO<string>(lessonFromDb.Content);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<string>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> UpdateLessonContentAsync(int lessonId, string content)
    {
        try
        {
            var lessonFromDb = await _unitOfWork.Lesson.GetAsync(
                l => l.Id.Equals(lessonId)
                ) ?? throw new Exception("Lesson not found");

            // update content field
            lessonFromDb.Content = content;

            await _unitOfWork.Lesson.UpdateAsync(lessonFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(lessonFromDb.Content);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Lesson Progress Tracking
    public async Task<ResponseDTO<double>> GetStudentProgressInLessonAsync(int lessonId, string studentId)
    {
        try
        {
            // get lesson progress
            var lessonProgress = await _unitOfWork.Progress.GetAsync(
                p => p.LessonId.Equals(lessonId) && p.StudentId.Equals(studentId)
                ) ?? throw new Exception("Progress not found!");

            double progressPercentage = lessonProgress.IsCompleted ? 100 : 0;

            return new ResponseDTO<double>(progressPercentage);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }
    public async Task<ResponseDTO<bool>> IsLessonCompletedAsync(int lessonId, string studentId)
    {
        try
        {
            // get lesson progress
            var lessonProgress = await _unitOfWork.Progress.GetAsync(
                p => p.LessonId.Equals(lessonId) && p.StudentId.Equals(studentId)
                ) ?? throw new Exception("Progress not found!");

            return new ResponseDTO<bool>(lessonProgress.IsCompleted);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> UpdateStudentProgressInLessonAsync(int lessonId, string studentId)
    {
        try
        {
            // get lesson progress
            var lessonProgress = await _unitOfWork.Progress.GetAsync(
                p => p.LessonId.Equals(lessonId) && p.StudentId.Equals(studentId)
                ) ?? throw new Exception("Progress not found!");

            // update LessonProgress completed
            lessonProgress.IsCompleted = true;

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Lesson Ordering and Prerequisites
    public async Task<ResponseDTO<int>> GetLessonOrderAsync(int lessonId)
    {
        try
        {
            var lesson = await _unitOfWork.Lesson.GetAsync(
                filter: l => l.Id.Equals(lessonId)
                ) ?? throw new Exception("Lesson not found!");

            return new ResponseDTO<int>(lesson.Order);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }

    // POST action
    public async Task<ResponseDTO<object>> UpdateLessonOrderAsync(int lessonId, int newOrder)
    {
        try
        {
            var lesson = await _unitOfWork.Lesson.GetAsync(
                filter: l => l.Id.Equals(lessonId)
                ) ?? throw new Exception("Lesson not found!");

            // update order for lesson
            lesson.Order = newOrder;

            await _unitOfWork.Lesson.UpdateAsync(lesson);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Lesson Statistics
    public async Task<ResponseDTO<int>> GetTotalLessonsInModuleAsync(int moduleId)
    {
        try
        {
            var moduleLessons = await _unitOfWork.Lesson.GetAllAsync(
                l => l.ModuleId.Equals(moduleId));

            return new ResponseDTO<int>(moduleLessons.Count());
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }
    public async Task<ResponseDTO<int>> GetTotalCompletedLessonsForStudentAsync(int moduleId, string studentId)
    {
        try
        {
            // get all module lessons and return count of lesson progress is completed for student
            var moduleLessons = await _unitOfWork.Lesson.GetAllAsync(
                l => l.ModuleId.Equals(moduleId));

            int completedLessonsCount = 0;
            foreach (var lesson in moduleLessons)
            {
                var lessonProgress = await _unitOfWork.Progress.GetAsync(
                    p => p.LessonId.Equals(lesson.Id)
                    ) ?? throw new Exception("Lesson progress not found!");

                if (lessonProgress.IsCompleted)
                    completedLessonsCount++;
            }

            return new ResponseDTO<int>(completedLessonsCount);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }
    public async Task<ResponseDTO<Dictionary<string, int>>> GetLessonCompletionStatsAsync(int moduleId)
    {
        try
        {
            // Step 1: Retrieve all lessons in the specified module
            var lessons = await _unitOfWork.Lesson.GetAllAsync(
                l => l.ModuleId.Equals(moduleId));

            // Step 2: Initialize the dictionary to hold the stats
            var completionStats = new Dictionary<string, int>();

            // Step 3: For each lesson, count the number of students who completed it
            foreach (var lesson in lessons)
            {
                var completedLessonProgresses = await _unitOfWork.Progress.GetAllAsync(
                    p => p.LessonId.Equals(lesson.Id) && 
                    p.IsCompleted);


                // Add the lesson title and the count to the dictionary
                completionStats[lesson.Title] = completedLessonProgresses.Count();
            }

            // Step 4: Return the completion stats in a ResponseDTO
            return new ResponseDTO<Dictionary<string, int>>(completionStats);
        }
        catch (Exception ex)
        {
            // Handle exceptions by returning a descriptive error message
            return new ResponseDTO<Dictionary<string, int>>(ex.Message);
        }
    }
    #endregion  
}
