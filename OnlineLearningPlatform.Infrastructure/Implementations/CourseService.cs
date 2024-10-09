using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class CourseService(IUnitOfWork unitOfWork, IMapper mapper) : ICourseService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region Course Management
    public async Task<ResponseDTO<IEnumerable<CourseDTO>>> GetAllCoursesAsync()
    {
        try
        {
            var allCourses = await _unitOfWork.Course.GetAllAsync(
                includeProperties: "Instructor,Modules,Enrollments,Quizzes");

            var mappedCourses = _mapper.Map<IEnumerable<CourseDTO>>(allCourses);

            return new ResponseDTO<IEnumerable<CourseDTO>>(mappedCourses);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<CourseDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<CourseDTO>> GetCourseByIdAsync(int courseId)
    {
        try
        {
            var course = await _unitOfWork.Course.GetAsync(
                filter: c => c.Id.Equals(courseId),
                includeProperties: "Instructor,Modules,Enrollments,Quizzes")
                ?? throw new Exception("Course not found!");

            var mappedCourse = _mapper.Map<CourseDTO>(course);

            return new ResponseDTO<CourseDTO>(mappedCourse);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<CourseDTO>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> CreateCourseAsync(CourseDTO courseDTO)
    {
        try
        {
            var courseForDb = _mapper.Map<Course>(courseDTO);

            // create and add
            await _unitOfWork.Course.AddAsync(courseForDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UpdateCourseAsync(CourseDTO courseDTO)
    {
        try
        {
            var courseFromDb = await _unitOfWork.Course.GetAsync(
                c => c.Id.Equals(courseDTO.Id)
                ) ?? throw new Exception("Course not found!");

            // mapping
            _mapper.Map(courseDTO, courseFromDb);

            // update and save
            await _unitOfWork.Course.UpdateAsync(courseFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> DeleteCourseAsync(CourseDTO courseDTO)
    {
        try
        {
            var courseFromDb = await _unitOfWork.Course.GetAsync(
                c => c.Id.Equals(courseDTO.Id)
                ) ?? throw new Exception("Course not found!");

            // update and save
            await _unitOfWork.Course.RemoveAsync(courseFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Course Searching and Filtering
    public async Task<ResponseDTO<IEnumerable<CourseDTO>>> SearchCoursesAsync(string keyword)
    {
        try
        {
            // Ensure the keyword is not null or empty for filtering
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new ResponseDTO<IEnumerable<CourseDTO>>([], "No keyword provided.");
            }

            // Fetch courses from the repository (assuming you have an appropriate method)
            var courses = await _unitOfWork.Course.GetAllAsync(
                includeProperties: "Instructor,Modules,Enrollments,Quizzes");

            // Filter courses based on keyword (e.g., search by Title, Description, Category)
            var filteredCourses = courses.Where(course =>
                course.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                course.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                course.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            // Map the filtered courses to CourseDTOs
            var courseDTOs = _mapper.Map<IEnumerable<CourseDTO>>(filteredCourses);

            return new ResponseDTO<IEnumerable<CourseDTO>>(courseDTOs);
        }
        catch (Exception ex)
        {
            // Return error response if any exception occurs
            return new ResponseDTO<IEnumerable<CourseDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<CourseDTO>>> GetCoursesByCategoryAsync(string category)
    {
        try
        {
            var filteredCourses = await _unitOfWork.Course.GetAllAsync(
                filter: c => c.Category.Equals(category),
                includeProperties: "Instructor,Modules,Enrollments,Quizzes");

            var mappedCourses = _mapper.Map<IEnumerable<CourseDTO>>(filteredCourses);

            return new ResponseDTO<IEnumerable<CourseDTO>>(mappedCourses);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<CourseDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<CourseDTO>>> GetCoursesByLevelAsync(string level)
    {
        try
        {
            var filteredCourses = await _unitOfWork.Course.GetAllAsync(
                filter: c => c.Level.Equals(level),
                includeProperties: "Instructor,Modules,Enrollments,Quizzes");

            var mappedCourses = _mapper.Map<IEnumerable<CourseDTO>>(filteredCourses);

            return new ResponseDTO<IEnumerable<CourseDTO>>(mappedCourses);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<CourseDTO>>(ex.Message);
        }
    }
    #endregion

    #region Course Enrollment and Access
    public async Task<ResponseDTO<bool>> IsUserEnrolledInCourseAsync(int courseId, string userId)
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
    public async Task<ResponseDTO<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByCourseAsync(int courseId)
    {
        try
        {
            var courseEnrollments = await _unitOfWork.Enrollment.GetAllAsync(
                filter: e => e.CourseId.Equals(courseId),
                includeProperties: "Student,Course");

            var mappedEnrollments = _mapper.Map<IEnumerable<EnrollmentDTO>>(courseEnrollments);

            return new ResponseDTO<IEnumerable<EnrollmentDTO>>(mappedEnrollments);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<EnrollmentDTO>>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> EnrollUserInCourseAsync(int courseId, string userId)
    {
        try
        {
            var courseFromDb = await _unitOfWork.Course.GetAsync(c => c.Id.Equals(courseId)) 
                ?? throw new Exception("Course not found");

            var userFromDb = await _unitOfWork.User.GetAsync(u => u.Id.Equals(userId)) 
                ?? throw new Exception("User not found");

            // prepare enrollment
            Enrollment enrollmentForDb = new()
            {
                CourseId = courseFromDb.Id,
                StudentId = userFromDb.Id,
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
    public async Task<ResponseDTO<object>> UnenrollUserFromCourseAsync(int courseId, string userId)
    {
        try
        {
            var enrollmentFromDb = await _unitOfWork.Enrollment.GetAsync(
                e => e.CourseId.Equals(courseId) && e.StudentId.Equals(userId)
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

    #region Course Modules and Content
    public async Task<ResponseDTO<IEnumerable<ModuleDTO>>> GetModulesByCourseAsync(int courseId)
    {
        try
        {
            var courseModules = await _unitOfWork.Module.GetAllAsync(
                filter: m => m.CourseId.Equals(courseId),
                includeProperties: "Course,Lessons");

            var mappedModules = _mapper.Map<IEnumerable<ModuleDTO>>(courseModules);

            return new ResponseDTO<IEnumerable<ModuleDTO>>(mappedModules);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<ModuleDTO>>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> AddModuleToCourseAsync(int courseId, ModuleDTO moduleDTO)
    {
        try
        {
            var courseFromDb = await _unitOfWork.Course.GetAsync(
                filter: c => c.Id.Equals(courseId),
                includeProperties: "Modules") ?? throw new Exception("Course not found!");

            // mapped module
            var mappedModule = _mapper.Map<Module>(moduleDTO);

            // adding module to course
            courseFromDb.Modules.Add(mappedModule);

            await _unitOfWork.Course.UpdateAsync(courseFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> RemoveModuleFromCourseAsync(int courseId, int moduleId)
    {
        try
        {
            var moduleFromDb = await _unitOfWork.Module.GetAsync(
                filter: m => m.Id.Equals(moduleId) && m.CourseId.Equals(courseId)
                ) ?? throw new Exception("Module not found!");

            await _unitOfWork.Module.RemoveAsync(moduleFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Instructor-Specific Features
    public async Task<ResponseDTO<bool>> IsCourseInstructorAsync(int courseId, string instructorId)
    {
        try
        {
            var existInstructorCourse = await _unitOfWork.Course.AnyAsync(
                c => c.Id.Equals(courseId) && c.InstructorId.Equals(instructorId));

            return new ResponseDTO<bool>(existInstructorCourse);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<CourseDTO>>> GetCoursesByInstructorAsync(string instructorId)
    {
        try
        {
            var instructorCourses = await _unitOfWork.Course.GetAllAsync(
                c => c.InstructorId.Equals(instructorId));

            var mappedCourses = _mapper.Map<IEnumerable<CourseDTO>>(instructorCourses);

            return new ResponseDTO<IEnumerable<CourseDTO>>(mappedCourses);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<CourseDTO>>(ex.Message);
        }
    }
    #endregion

    #region Course Statistics and Insights
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
}
