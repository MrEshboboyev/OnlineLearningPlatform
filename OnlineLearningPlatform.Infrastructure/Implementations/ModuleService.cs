using AutoMapper;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Implementations;

public class ModuleService(IUnitOfWork unitOfWork, IMapper mapper,
    ILessonService lessonService) : IModuleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILessonService _lessonService = lessonService;

    #region Module Management
    public async Task<ResponseDTO<IEnumerable<ModuleDTO>>> GetAllModulesAsync()
    {
        try
        {
            var allModules = await _unitOfWork.Module.GetAllAsync(
                includeProperties: "Course,Lessons");

            var mappedModules = _mapper.Map<IEnumerable<ModuleDTO>>(allModules);

            return new ResponseDTO<IEnumerable<ModuleDTO>>(mappedModules);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<ModuleDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<IEnumerable<ModuleDTO>>> GetModulesByCourseIdAsync(int courseId)
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
    public async Task<ResponseDTO<IEnumerable<ModuleDTO>>> GetModulesByInstructorAsync(string instructorId)
    {
        try
        {
            var instructorModules = await _unitOfWork.Module.GetAllAsync(
                filter: m => m.Course.InstructorId.Equals(instructorId),
                includeProperties: "Course,Lessons");

            var mappedModules = _mapper.Map<IEnumerable<ModuleDTO>>(instructorModules);

            return new ResponseDTO<IEnumerable<ModuleDTO>>(mappedModules);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<ModuleDTO>>(ex.Message);
        }
    }
    public async Task<ResponseDTO<ModuleDTO>> GetModuleByIdAsync(int moduleId)
    {
        try
        {
            var module = await _unitOfWork.Module.GetAsync(
                filter: m => m.Id.Equals(moduleId),
                includeProperties: "Course,Lessons");

            var mappedModule = _mapper.Map<ModuleDTO>(module);

            return new ResponseDTO<ModuleDTO>(mappedModule);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<ModuleDTO>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> CreateModuleAsync(ModuleDTO moduleDTO)
    {
        try
        {
            var moduleForDb = _mapper.Map<Module>(moduleDTO);

            // create and save
            await _unitOfWork.Module.AddAsync(moduleForDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Module created successfully!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> UpdateModuleAsync(ModuleDTO moduleDTO)
    {
        try
        {
            var moduleFromDb = await _unitOfWork.Module.GetAsync(
                m => m.Id.Equals(moduleDTO.Id)
                ) ?? throw new Exception("Module not found!");

            // mapping fields
            _mapper.Map(moduleDTO, moduleFromDb);

            // update and save
            await _unitOfWork.Module.UpdateAsync(moduleFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Module updated successfully!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> DeleteModuleAsync(ModuleDTO moduleDTO)
    {
        try
        {
            var moduleFromDb = await _unitOfWork.Module.GetAsync(
                m => m.Id.Equals(moduleDTO.Id)
                ) ?? throw new Exception("Module not found!");

            // remove and save
            await _unitOfWork.Module.RemoveAsync(moduleFromDb);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO<object>(null, "Module deleted successfully!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Lesson Management within Modules
    public async Task<ResponseDTO<IEnumerable<LessonDTO>>> GetLessonsByModuleAsync(int moduleId)
    {
        try
        {
            var moduleLessons = await _unitOfWork.Lesson.GetAllAsync(
                l => l.ModuleId.Equals(moduleId),
                includeProperties: "Module");

            var mappedLessons = _mapper.Map<IEnumerable<LessonDTO>>(moduleLessons);

            return new ResponseDTO<IEnumerable<LessonDTO>>(mappedLessons);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<IEnumerable<LessonDTO>>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> AddLessonToModuleAsync(int moduleId, LessonDTO lessonDTO)
    {
        try
        {
            lessonDTO.ModuleId = moduleId;
            await _lessonService.CreateLessonAsync(lessonDTO);

            return new ResponseDTO<object>(null, "Add lesson to module!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    public async Task<ResponseDTO<object>> RemoveLessonFromModuleAsync(int moduleId, int lessonId)
    {
        try
        {
            LessonDTO lessonDTO = new()
            {
                ModuleId = moduleId,
                Id = lessonId
            };
            await _lessonService.DeleteLessonAsync(lessonDTO);

            return new ResponseDTO<object>(null, "Remove lesson to module!");
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Module Status and Ordering
    public async Task<ResponseDTO<int>> GetModuleOrderAsync(int moduleId)
    {
        try
        {
            var module = await _unitOfWork.Module.GetAsync(m => m.Id.Equals(moduleId)
                ) ?? throw new Exception("Module not found!");

            return new ResponseDTO<int>(module.Order);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }
    public async Task<ResponseDTO<bool>> IsModuleCompletedAsync(int moduleId, string studentId)
    {
        try
        {
            var module = await _unitOfWork.Module.GetAsync(m => m.Id.Equals(moduleId),
                includeProperties: "Lessons") ?? throw new Exception("Module not found!");

            var moduleLessons = module.Lessons;

            // getting completed lessonIds for student
            var studentCompletedProgressesForModule = await _unitOfWork.Progress.GetAllAsync(
                p => p.StudentId.Equals(studentId) &&
                p.Lesson.ModuleId.Equals(moduleId) &&
                p.IsCompleted);

            var completedLessonIds = studentCompletedProgressesForModule.Select(p => p.LessonId);

            // Check if all lessons are completed
            bool isCompleted = module.Lessons.All(lesson => completedLessonIds.Contains(lesson.Id));

            return new ResponseDTO<bool>(isCompleted);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<bool>(ex.Message);
        }
    }

    // POST actions
    public async Task<ResponseDTO<object>> UpdateModuleOrderAsync(int moduleId, int newOrder)
    {
        try
        {
            var module = await _unitOfWork.Module.GetAsync(m => m.Id.Equals(moduleId)
                ) ?? throw new Exception("Module not found!");

            // update module order
            module.Order = newOrder;

            return new ResponseDTO<object>(null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<object>(ex.Message);
        }
    }
    #endregion

    #region Module Progress Tracking
    public async Task<ResponseDTO<double>> GetModuleProgressAsync(int moduleId, string studentId)
    {
        try
        {
            var module = await _unitOfWork.Module.GetAsync(
                m => m.Id.Equals(moduleId),
                includeProperties: "Lessons") ?? throw new Exception("Module not found!");

            var totalModuleLessons = module.Lessons.Count;

            // getting completed lessons for student
            var studentCompletedProgressesForModule = await _unitOfWork.Progress.GetAllAsync(
                p => p.StudentId.Equals(studentId) &&
                p.Lesson.ModuleId.Equals(moduleId) &&
                p.IsCompleted);

            var completedLessons = studentCompletedProgressesForModule.Select(p => p.Lesson);

            double progressPercentage = ((double)completedLessons.Count() / totalModuleLessons) * 100;

            return new ResponseDTO<double>(progressPercentage);
        }
        catch (Exception ex)
        {
            return new ResponseDTO<double>(ex.Message);
        }
    }
    #endregion

    #region Module Statistics
    public async Task<ResponseDTO<int>> GetTotalLessonsInModuleAsync(int moduleId)
    {
        try
        {
            var moduleLessons = await _unitOfWork.Lesson.GetAllAsync(
                l => l.ModuleId.Equals(moduleId));

            return new ResponseDTO<int>(moduleLessons.Count());
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }
    public async Task<ResponseDTO<int>> GetTotalModulesInCourseAsync(int courseId)
    {
        try
        {
            var courseModules = await _unitOfWork.Module.GetAllAsync(
                m => m.CourseId.Equals(courseId));

            return new ResponseDTO<int>(courseModules.Count());
        }
        catch (Exception ex)
        {
            return new ResponseDTO<int>(ex.Message);
        }
    }
    #endregion
}
