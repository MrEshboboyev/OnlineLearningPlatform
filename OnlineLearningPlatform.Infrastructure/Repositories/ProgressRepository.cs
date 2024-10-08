using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class ProgressRepository(AppDbContext db) : Repository<Progress>(db),
        IProgressRepository
    {
    }
}
