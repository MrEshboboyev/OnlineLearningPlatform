using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class QuizAttemptRepository(AppDbContext db) : Repository<QuizAttempt>(db),
        IQuizAttemptRepository
    {
    }
}
