using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.Services;
using OnlineLearningPlatform.Application.Services.Interfaces;
using OnlineLearningPlatform.Infrastructure.Data;
using OnlineLearningPlatform.Infrastructure.Implementations;
using OnlineLearningPlatform.Infrastructure.Repositories;

namespace OnlineLearningPlatform.Infrastructure.Configurations
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // adding lifetimes
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IProgressService, ProgressService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuizSubmissionService, QuizSubmissionService>();

            // Hangfire PostgreSQL Configuration
            var hangfireConnectionString = configuration.GetConnectionString("HangfireConnection");
            if (string.IsNullOrEmpty(hangfireConnectionString))
                throw new ArgumentNullException(nameof(hangfireConnectionString));

            services.AddHangfireServices(hangfireConnectionString);

            return services;
        }
    }
}
