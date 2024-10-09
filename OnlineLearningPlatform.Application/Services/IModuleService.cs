using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface IModuleService
{
    #region Module Management
    Task<ResponseDTO<IEnumerable<ModuleDTO>>> GetAllModulesAsync();
    Task<ResponseDTO<IEnumerable<ModuleDTO>>> GetModulesByCourseIdAsync(int courseId);
    Task<ResponseDTO<ModuleDTO>> GetModuleByIdAsync(int moduleId);

    // POST actions
    Task<ResponseDTO<object>> CreateModuleAsync(ModuleDTO moduleDTO);
    Task<ResponseDTO<object>> UpdateModuleAsync(ModuleDTO moduleDTO);
    Task<ResponseDTO<object>> DeleteModuleAsync(ModuleDTO moduleDTO);
    #endregion

    #region Lesson Management within Modules
    Task<ResponseDTO<IEnumerable<LessonDTO>>> GetLessonsByModuleAsync(int moduleId);

    // POST actions
    Task<ResponseDTO<object>> AddLessonToModuleAsync(int moduleId, LessonDTO lessonDTO);
    Task<ResponseDTO<object>> RemoveLessonFromModuleAsync(int moduleId, int lessonId);
    #endregion

    #region Module Status and Ordering
    Task<ResponseDTO<int>> GetModuleOrderAsync(int moduleId);
    Task<ResponseDTO<bool>> IsModuleCompletedAsync(int moduleId, string studentId);

    // POST actions
    Task<ResponseDTO<object>> UpdateModuleOrderAsync(int moduleId, int newOrder);
    #endregion

    #region Module Progress Tracking
    Task<ResponseDTO<double>> GetModuleProgressAsync(int moduleId, string studentId);
    #endregion

    #region Module Statistics
    Task<ResponseDTO<int>> GetTotalLessonsInModuleAsync(int moduleId);
    Task<ResponseDTO<int>> GetTotalModulesInCourseAsync(int courseId);
    #endregion
}
