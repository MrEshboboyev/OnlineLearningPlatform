using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class LessonRepository(AppDbContext db) : Repository<Lesson>(db),
        ILessonRepository
    {
    }
}
