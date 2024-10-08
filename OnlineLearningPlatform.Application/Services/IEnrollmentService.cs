using OnlineLearningPlatform.Application.DTOs;

namespace OnlineLearningPlatform.Application.Services;

public interface IEnrollmentService
{
    #region Enrollment Management
    Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetAllEnrollmentsAsync();
    Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByStudentAsync(string studentId);

    // POST actions
    Task<ResponseDTO<object>> EnrollStudentInCourseAsync(int courseId, string studentId);
    Task<ResponseDTO<object>> UnenrollStudentInCourseAsync(int courseId, string studentId);
    #endregion

    #region Enrollment Status and Validation
    Task<ResponseDTO<bool>> IsStudentEnrolledInCourseAsync(int courseId, string userId);
    Task<ResponseDTO<DateTime>> GetEnrollmentDateAsync(int courseId, string userId);
    #endregion

    #region Progress Tracking
    Task<ResponseDTO<double>> GetStudentProgressAsync(int courseId, string studentId);

    // POST actions
    Task<ResponseDTO<object>> UpdateStudentProgressAsync(int courseId, string studentId, double progressPercentage);
    #endregion

    #region Completion and Achievement
    Task<ResponseDTO<bool>> IsCourseCompletedAsync(int courseId, string studentId);

    // POST actions
    Task<ResponseDTO<object>> MarkCourseIsCompletedAsync(int courseId, string studentId);
    #endregion

    #region Enrollment Statistics
    Task<ResponseDTO<int>> GetTotalEnrollmentsInCourseAsync(int courseId);
    Task<ResponseDTO<Dictionary<string, int>>> GetEnrollmentStatisticsAsync()
    #endregion

    #region Enrollment History
    Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetEnrollmentHistory(string studentId);
    #endregion

    #region Batch Enrollment
    // POST actions
    Task<ResponseDTO<object>> EnrollStudentsInCourseBatchAsync(int courseId, List<string> studentIds);
    #endregion
}
