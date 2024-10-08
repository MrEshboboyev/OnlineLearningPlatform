using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface ICourseService
{
    #region Course Management
    Task<ResponseDTO<IEnumerable<CourseDTO>>> GetAllCoursesAsync();
    Task<ResponseDTO<CourseDTO>> GetCourseByIdAsync(int courseId);

    // POST actions
    Task<ResponseDTO<object>> CreateCourseAsync(CourseDTO courseDTO);
    Task<ResponseDTO<object>> UpdateCourseAsync(CourseDTO courseDTO);
    Task<ResponseDTO<object>> DeleteCourseAsync(CourseDTO courseDTO);
    #endregion

    #region Course Searching and Filtering
    Task<ResponseDTO<IEnumerable<CourseDTO>>> SearchCoursesAsync(string keyword);
    Task<ResponseDTO<IEnumerable<CourseDTO>>> GetCoursesByCategoryAsync(string category);
    Task<ResponseDTO<IEnumerable<CourseDTO>>> GetCoursesByLevelAsync(string level);
    #endregion

    #region Course Enrollment and Access
    Task<ResponseDTO<bool>> IsUserEnrolledInCourseAsync(int courseId, string userId);
    Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByCourseAsync(int courseId);

    // POST actions
    Task<ResponseDTO<object>> EnrollUserInCourseAsync(int courseId, string userId);
    Task<ResponseDTO<object>> UnenrollUserFromCourseAsync(int courseId, string userId);
    #endregion

    #region Course Modules and Content
    Task<ResponseDTO<IEnumerable<ModuleDTO>>> GetModulesByCourseAsync(int courseId);

    // POST actions
    Task<ResponseDTO<object>> AddModuleToCourseAsync(int courseId, ModuleDTO moduleDTO);
    Task<ResponseDTO<object>> RemoveModuleFromCourseAsync(int courseId, int moduleId);
    #endregion

    #region Instructor-Specific Features
    Task<ResponseDTO<bool>> IsCourseInstructorAsync(int courseId, string instructorId); 
    Task<ResponseDTO<IEnumerable<CourseDTO>>> GetCoursesByInstructorAsync(string instructorId);
    #endregion

    #region Course Statistics and Insights
    Task<ResponseDTO<int>> GetTotalEnrollmentsInCourseAsync(int courseId);
    Task<ResponseDTO<Dictionary<string, int>>> GetCourseCompletionStatsAsync(int courseId);
    Task<ResponseDTO<double>> GetCourseRatingAsync(int courseId);
    #endregion

    #region Course Publishing and Status
    Task<ResponseDTO<bool>> IsCoursePublishedAsync(int courseId);

    // POST actions
    Task<ResponseDTO<object>> PublishCourseAsync(int courseId);
    Task<ResponseDTO<object>> UnpublishCourseAsync(int courseId);
    #endregion

    #region Course Content Preview and Recommendations
    Task<ResponseDTO<IEnumerable<CourseDTO>>> GetRecommendedCoursesAsync(string userId);
    Task<ResponseDTO<CourseDTO>> GetCoursePreviewAsync(int courseId);
    #endregion

    #region Course Moderation and Approval (if needed)
    // POST actions
    Task<ResponseDTO<object>> ApproveCourseAsync(int courseId);
    Task<ResponseDTO<object>> RejectCourseAsync(int courseId, string reason);
    #endregion
}
