using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class ModuleRepository(AppDbContext db) : Repository<Module>(db),
        IModuleRepository
    {
    }
}
