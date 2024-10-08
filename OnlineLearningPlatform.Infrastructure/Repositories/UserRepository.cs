using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext db) : Repository<AppUser>(db),
        IUserRepository
    {
        private readonly AppDbContext _db = db;
    }
}
