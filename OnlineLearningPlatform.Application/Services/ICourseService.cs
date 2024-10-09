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
    #endregion
}
