using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Services;

public interface IProgressService
{
    #region Progress Tracking
    Task<ResponseDTO<ProgressDTO>> GetStudentProgressInLessonAsync(string studentId, int lessonId);
    Task<ResponseDTO<IEnumerable<ProgressDTO>>> GetStudentProgressInCourseAsync(string studentId, int courseId);
    Task<ResponseDTO<IEnumerable<ProgressDTO>>> GetProgressesInCourseAsync(int courseId);
    Task<ResponseDTO<double>> GetOverallProgressInCourseAsync(string studentId, int courseId);
    #endregion

    #region Marking Progress
    // POST actions
    Task<ResponseDTO<object>> MarkLessonAsCompletedAsync(string studentId, int lessonId);
    Task<ResponseDTO<object>> UnmarkLessonAsCompletedAsync(string studentId, int lessonId);
    Task<ResponseDTO<object>> UpdateCompletionDateAsync(string studentId, int lessonId, DateTime completionDate);
    #endregion

    #region Progress Validation
    Task<ResponseDTO<bool>> IsLessonCompletedAsync(string studentId, int lessonId);
    Task<ResponseDTO<DateTime?>> GetLessonCompletionDateAsync(string studentId, int lessonId);
    #endregion

    #region Progress History
    Task<ResponseDTO<IEnumerable<ProgressDTO>>> GetProgressHistoryForStudentAsync(string studentId);
    #endregion

    #region Course Completion
    Task<ResponseDTO<bool>> IsCourseFullyCompletedAsync(string studentId, int courseId);
    Task<ResponseDTO<DateTime?>> GetCourseCompletionDateAsync(string studentId, int courseId);
    #endregion

    #region Progress Statistics
    Task<ResponseDTO<int>> GetCompletedLessonsCountAsync(string studentId, int courseId);
    Task<ResponseDTO<Dictionary<string, double>>> GetCourseCompletionStatisticsAsync();
    #endregion
    Task UpdateProgressForNewLesson(int moduleId, Lesson lesson);
}
