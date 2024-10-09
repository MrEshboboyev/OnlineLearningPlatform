using AutoMapper;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region UserProfile
            CreateMap<UserProfile, UserProfileDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.User, opt => opt.Ignore());
            #endregion

            #region AppUser

            // AppUser -> UserDTO
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ReverseMap()
                    .ForMember(dest => dest.UserProfile, opt => opt.Ignore())
                    .ForMember(dest => dest.CoursesTaught, opt => opt.Ignore())
                    .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
                    .ForMember(dest => dest.QuizSubmissions, opt => opt.Ignore());

            // fix this coming soon (admin panel)
            // AppUser -> UserActivityDTO
            CreateMap<AppUser, UserActivityDTO>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
            #endregion

            #region Course

            // Course -> CourseDTO
            CreateMap<Course, CourseDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.Modules, opt => opt.Ignore())
                    .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
                    .ForMember(dest => dest.Quizzes, opt => opt.Ignore());
            #endregion

            #region Enrollment

            // Enrollment -> EnrollmentDTO
            CreateMap<Enrollment, EnrollmentDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.Course, opt => opt.Ignore())
                    .ForMember(dest => dest.Student, opt => opt.Ignore());
            #endregion

            #region Module

            // Module -> ModuleDTO
            CreateMap<Module, ModuleDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.Course, opt => opt.Ignore())
                    .ForMember(dest => dest.Lessons, opt => opt.Ignore());
            #endregion

            #region Lesson

            // Lesson -> LessonDTO
            CreateMap<Lesson, LessonDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.Module, opt => opt.Ignore());
            #endregion

            #region Quiz

            // Quiz -> QuizDTO
            CreateMap<Quiz, QuizDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.Course, opt => opt.Ignore())
                    .ForMember(dest => dest.Questions, opt => opt.Ignore())
                    .ForMember(dest => dest.QuizSubmissions, opt => opt.Ignore());
            #endregion

            #region Question

            // Question -> QuestionDTO
            CreateMap<Question, QuestionDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.Quiz, opt => opt.Ignore())
                    .ForMember(dest => dest.Answers, opt => opt.Ignore());
            #endregion
        }
    }
}
