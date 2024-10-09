using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper) : IEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region Enrollment Management
    public async Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetAllEnrollmentsAsync()
    {
        try
        {
            var allEnrollments = await _unitOfWork.Enrollment.GetAllAsync(
                includeProperties: "Course,Student");

            var mappedEnrollments = _mapper.Map<IEnumerable<EnrollmentDTO>>(allEnrollments);

            return new ResponseDTO<IEnumerable<EnrollmentDTO>>(mappedEnrollments);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<EnrollmentDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByStudentAsync(string studentId)
    {
        try
        {
            var allEnrollments = await _unitOfWork.Enrollment.GetAllAsync(
                filter: e => e.StudentId.Equals(studentId),
                includeProperties: "Course,Student");

            var mappedEnrollments = _mapper.Map<IEnumerable<EnrollmentDTO>>(allEnrollments);

            return new ResponseDTO<IEnumerable<EnrollmentDTO>>(mappedEnrollments);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<EnrollmentDTO>>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> EnrollStudentInCourseAsync(int courseId, string studentId)
    {
        try
        {
            var courseFromDb = await _unitOfWork.Course.GetAsync(c => c.Id.Equals(courseId))
                ?? throw new Exception("Course not found");

            var studentFromDb = await _unitOfWork.User.GetAsync(u => u.Id.Equals(studentId))
                ?? throw new Exception("Student not found");

            // prepare enrollment
            Enrollment enrollmentForDb = new()
            {
                CourseId = courseFromDb.Id,
                StudentId = studentFromDb.Id,
                EnrollmentDate = DateTime.Now
            };

            await _unitOfWork.Enrollment.AddAsync(enrollmentForDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UnenrollStudentFromCourseAsync(int courseId, string studentId)
    {
        try
        {
            var enrollmentFromDb = await _unitOfWork.Enrollment.GetAsync(
                e => e.CourseId.Equals(courseId) && e.StudentId.Equals(studentId)
                ) ?? throw new Exception("Enrollment not found!"); 

            await _unitOfWork.Enrollment.RemoveAsync(enrollmentFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Enrollment Status and Validation
    public async Task<ResponseDTO<bool>> IsStudentEnrolledInCourseAsync(int courseId, string userId)
    {
        try
        {
            var enrollmentExist = await _unitOfWork.Enrollment.AnyAsync(
                e => e.CourseId.Equals(courseId) && e.StudentId.Equals(userId));

            return new ResponseDTO<bool>(enrollmentExist);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    public async Task<ResponseDTO<DateTime>> GetEnrollmentDateAsync(int courseId, string userId)
    {
        try
        {
            var enrollment = await _unitOfWork.Enrollment.GetAsync(
                e => e.CourseId.Equals(courseId) && e.StudentId.Equals(userId)
                ) ?? throw new Exception("Enrollment not found!");

            return new ResponseDTO<DateTime>(enrollment.EnrollmentDate);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<DateTime>(ex.Message);
        }
    }
    #endregion

    #region Enrollment Statistics
    public async Task<ResponseDTO<int>> GetTotalEnrollmentsInCourseAsync(int courseId)
    {
        try
        {
            var courseEnrollments = await _unitOfWork.Enrollment.GetAllAsync(
                filter: e => e.CourseId.Equals(courseId),
                includeProperties: "Course,Student");

            return new ResponseDTO<int>(courseEnrollments.Count());
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }
    #endregion

    #region Enrollment History
    public async Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetEnrollmentHistory(string studentId)
    {
        try
        {
            var userEnrollments = await _unitOfWork.Enrollment.GetAllAsync(
                e => e.StudentId.Equals(studentId),
                includeProperties: "Course,Student");

            var mappedEnrollments = _mapper.Map<IEnumerable<EnrollmentDTO>>(userEnrollments);

            return new ResponseDTO<IEnumerable<EnrollmentDTO>>(mappedEnrollments);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<EnrollmentDTO>>(ex.Message);
        }
    }
    #endregion

    #region Batch Enrollment
    // POST actions
    public async Task<ResponseDTO<object>> EnrollStudentsInCourseBatchAsync(int courseId, List<string> studentIds)
    {
        try
        {
            var course = await _unitOfWork.Course.GetAsync(
                c => c.Id.Equals(courseId),
                includeProperties: "Enrollments")
                ?? throw new Exception("Course not found!");

            // each user enrolled to this course
            // create enrollment and added to course enrollments
            foreach (var studentId in studentIds)
            {
                // create new enrollment
                Enrollment enrollment = new()
                {
                    StudentId = studentId,
                    CourseId = course.Id
                };

                course.Enrollments.Add(enrollment);
            }

            // update and save
            await _unitOfWork.Course.UpdateAsync(course);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion
}
