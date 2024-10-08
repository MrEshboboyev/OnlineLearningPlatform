using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class CourseRepository(AppDbContext db) : Repository<Course>(db),
        ICourseRepository
    {
    }
}
