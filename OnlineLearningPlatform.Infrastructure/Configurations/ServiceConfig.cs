using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineLearningPlatform.Application.Common.Interfaces;
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

            return services;
        }
    }
}
