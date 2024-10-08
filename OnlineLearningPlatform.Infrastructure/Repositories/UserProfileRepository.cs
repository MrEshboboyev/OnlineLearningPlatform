using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class UserProfileRepository(AppDbContext db) : Repository<UserProfile>(db), 
        IUserProfileRepository
    {
        private readonly AppDbContext _db = db;
    }
}
