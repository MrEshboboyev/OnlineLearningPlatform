using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class UnitOfWork(AppDbContext db) : IUnitOfWork
    {
        private readonly AppDbContext _db = db;

        public IUserProfileRepository UserProfile { get; private set; } = new UserProfileRepository(db);
        public IUserRepository User { get; private set; } = new UserRepository(db);

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
