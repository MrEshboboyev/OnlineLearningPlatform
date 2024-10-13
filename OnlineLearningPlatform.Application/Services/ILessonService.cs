using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface ILessonService
{
    #region Lesson Management
    Task<ResponseDTO<IEnumerable<LessonDTO>>> GetAllLessonsAsync();
    Task<ResponseDTO<IEnumerable<LessonDTO>>> GetLessonsByModuleAsync(int moduleId);
    Task<ResponseDTO<IEnumerable<LessonDTO>>> GetLessonsByInstructorAsync(string instructorId);
    Task<ResponseDTO<LessonDTO>> GetLessonByIdAsync(int lessonId);

    // POST actions
    Task<ResponseDTO<object>> CreateLessonAsync(LessonDTO lessonDTO);
    Task<ResponseDTO<object>> UpdateLessonAsync(LessonDTO lessonDTO);
    Task<ResponseDTO<object>> DeleteLessonAsync(LessonDTO lessonDTO);
    #endregion

    #region Lesson Content Management
    Task<ResponseDTO<string>> GetLessonContentAsync(int lessonId);

    // POST actions
    Task<ResponseDTO<object>> UpdateLessonContentAsync(int lessonId, string content);
    #endregion

    #region Lesson Progress Tracking
    Task<ResponseDTO<double>> GetStudentProgressInLessonAsync(int lessonId, string studentId);
    Task<ResponseDTO<bool>> IsLessonCompletedAsync(int lessonId, string studentId);

    // POST actions
    Task<ResponseDTO<object>> UpdateStudentProgressInLessonAsync(int lessonId, string studentId);
    #endregion

    #region Lesson Ordering and Prerequisites
    Task<ResponseDTO<int>> GetLessonOrderAsync(int lessonId);

    // POST action
    Task<ResponseDTO<object>> UpdateLessonOrderAsync(int lessonId, int newOrder);
    #endregion

    #region Lesson Statistics
    Task<ResponseDTO<int>> GetTotalLessonsInModuleAsync(int moduleId);
    Task<ResponseDTO<int>> GetTotalCompletedLessonsForStudentAsync(int moduleId, string studentId);
    Task<ResponseDTO<Dictionary<string, int>>> GetLessonCompletionStatsAsync(int moduleId);
    #endregion
}
