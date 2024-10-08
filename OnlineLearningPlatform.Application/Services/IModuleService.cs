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
    Task<ResponseDTO<bool>> IsModulCompletedAsync(int moduleId, string studentId);

    // POST actions
    Task<ResponseDTO<object>> UpdateModuleOrderAsync(int moduleId, int newOrder);
    #endregion

    #region Module Progress Tracking
    Task<ResponseDTO<double>> GetModuleProgressAsync(int moduleId, string studentId);

    // POST actions
    Task<ResponseDTO<object>> UpdateModuleProgressAsync(int moduleId, string studentId, 
        double progressPercentage);
    #endregion

    #region Module Activation and Publishing
    Task<ResponseDTO<bool>> IsModulePublishedAsync(int moduleId);

    // POST actions
    Task<ResponseDTO<object>> PublishModuleAsync(int moduleId);
    Task<ResponseDTO<object>> UnpublishModuleAsync(int moduleId);
    #endregion

    #region Module Statistics
    Task<ResponseDTO<int>> GetTotalLessonsInModuleAsync(int moduleId);
    Task<ResponseDTO<int>> GetTotalModulesInCourseAsync(int courseId);
    Task<ResponseDTO<Dictionary<string, int>>> GetModuleCompletionStatsAsync(int moduleId);
    #endregion

    #region Module Prerequisites
    Task<ResponseDTO<ModuleDTO>> GetPrerequisitesForModuleAsync(int moduleId);

    // POST actions
    Task<ResponseDTO<object>> AddPrerequisiteToModuleAsync(int moduleId, int prerequisiteModuleId);
    Task<ResponseDTO<object>> RemovePrerequisiteFromModuleAsync(int moduleId, int prerequisiteModuleId);
    #endregion

    #region Module Content Preview
    Task<ResponseDTO<ModuleDTO>> GetModulePreviewAsync(int moduleId);
    #endregion

    #region Module Duplication
    Task<ResponseDTO<object>> DuplicateModuleAsync();
    #endregion
}
