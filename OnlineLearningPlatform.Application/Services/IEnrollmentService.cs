using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface IEnrollmentService
{
    #region Enrollment Management
    Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetAllEnrollmentsAsync();
    Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByStudentAsync(string studentId);

    // POST actions
    Task<ResponseDTO<object>> EnrollStudentInCourseAsync(int courseId, string studentId);
    Task<ResponseDTO<object>> UnenrollStudentFromCourseAsync(int courseId, string studentId);
    #endregion

    #region Enrollment Status and Validation
    Task<ResponseDTO<bool>> IsStudentEnrolledInCourseAsync(int courseId, string userId);
    Task<ResponseDTO<DateTime>> GetEnrollmentDateAsync(int courseId, string userId);
    #endregion

    #region Enrollment Statistics
    Task<ResponseDTO<int>> GetTotalEnrollmentsInCourseAsync(int courseId);
    #endregion

    #region Enrollment History
    Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetEnrollmentHistory(string studentId);
    #endregion

    #region Batch Enrollment
    // POST actions
    Task<ResponseDTO<object>> EnrollStudentsInCourseBatchAsync(int courseId, List<string> studentIds);
    #endregion
}
