﻿using AutoMapper;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Domain.Entities;
using System.Runtime.CompilerServices;

namespace OnlineLearningPlatform.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region UserProfile
            CreateMap<UserProfile, UserProfileDTO>()
                .ForMember(dest => dest.UserDTO, opt => opt.MapFrom(src => src.User))
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
                .ForMember(dest => dest.InstructorDTO, opt => opt.MapFrom(src => src.Instructor))
                .ReverseMap()
                    .ForMember(dest => dest.Modules, opt => opt.Ignore())
                    .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
                    .ForMember(dest => dest.Quizzes, opt => opt.Ignore());
            #endregion

            #region Enrollment

            // Enrollment -> EnrollmentDTO
            CreateMap<Enrollment, EnrollmentDTO>()
                .ForMember(dest => dest.CourseDTO, opt => opt.MapFrom(src => src.Course))
                .ForMember(dest => dest.StudentDTO, opt => opt.MapFrom(src => src.Student))
                .ReverseMap()
                    .ForMember(dest => dest.Course, opt => opt.Ignore())
                    .ForMember(dest => dest.Student, opt => opt.Ignore());
            #endregion

            #region Module

            // Module -> ModuleDTO
            CreateMap<Module, ModuleDTO>()
                .ForMember(dest => dest.CourseDTO, opt => opt.MapFrom(src => src.Course))
                .ForMember(dest => dest.LessonDTOs, opt => opt.MapFrom(src => src.Lessons))
                .ReverseMap()
                    .ForMember(dest => dest.Course, opt => opt.Ignore())
                    .ForMember(dest => dest.Lessons, opt => opt.Ignore());
            #endregion

            #region Lesson

            // Lesson -> LessonDTO
            CreateMap<Lesson, LessonDTO>()
                .ForMember(dest => dest.ModuleDTO, opt => opt.MapFrom(src => src.Module))
                .ReverseMap()
                    .ForMember(dest => dest.Module, opt => opt.Ignore());
            #endregion

            #region Quiz

            // Quiz -> QuizDTO
            CreateMap<Quiz, QuizDTO>()
                .ForMember(dest => dest.QuestionDTOs, opt => opt.MapFrom(quiz => quiz.Questions))
                .ForMember(dest => dest.CourseDTO, opt => opt.MapFrom(quiz => quiz.Course))
                .ForMember(dest => dest.QuizSubmissionDTOs, opt => opt.MapFrom(quiz => quiz.QuizSubmissions))
                .ReverseMap()
                    .ForMember(dest => dest.Course, opt => opt.Ignore())
                    .ForMember(dest => dest.Questions, opt => opt.Ignore())
                    .ForMember(dest => dest.QuizSubmissions, opt => opt.Ignore());
            #endregion

            #region Question

            // Question -> QuestionDTO
            CreateMap<Question, QuestionDTO>()
                .ForMember(dest => dest.QuizDto, opt => opt.MapFrom(src => src.Quiz))
                .ForMember(dest => dest.AnswerDTOs, opt => opt.MapFrom(src => src.Answers))
                .ReverseMap()
                    .ForMember(dest => dest.Quiz, opt => opt.Ignore())
                    .ForMember(dest => dest.Answers, opt => opt.Ignore());
            #endregion

            #region Answer

            // Answer -> AnswerDTO
            CreateMap<Answer, AnswerDTO>()
                .ForMember(dest => dest.QuestionDTO, opt => opt.MapFrom(src => src.Question))
                .ReverseMap()
                    .ForMember(dest => dest.Question, opt => opt.Ignore());
            #endregion

            #region QuizSubmission

            // QuizSubmission -> QuizSubmissionDTO
            CreateMap<QuizSubmission, QuizSubmissionDTO>()
                .ForMember(dest => dest.QuizDTO, opt => opt.MapFrom(src => src.Quiz))
                .ReverseMap()
                    .ForMember(dest => dest.Student, opt => opt.Ignore())
                    .ForMember(dest => dest.Quiz, opt => opt.Ignore());
            #endregion

            #region QuizAttempt

            // QuizAttempt -> QuizAttemptDTO (Reverse)
            CreateMap<QuizAttempt, QuizAttemptDTO>()
                .ForMember(dest => dest.QuizDTO, opt => opt.MapFrom(opt => opt.Quiz))
                .ForMember(dest => dest.UserDTO, opt => opt.MapFrom(opt => opt.User))
                .ReverseMap()
                    .ForMember(dest => dest.Quiz, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore());
            #endregion

            #region Progress

            // Progress -> ProgressDTO
            CreateMap<Progress, ProgressDTO>()
                .ForMember(dest => dest.StudentDTO, opt => opt.MapFrom(src => src.Student))
                .ForMember(dest => dest.LessonDTO, opt => opt.MapFrom(src => src.Lesson))
                .ReverseMap()
                    .ForMember(dest => dest.Student, opt => opt.Ignore())
                    .ForMember(dest => dest.Lesson, opt => opt.Ignore());
            #endregion
        }
    }
}
