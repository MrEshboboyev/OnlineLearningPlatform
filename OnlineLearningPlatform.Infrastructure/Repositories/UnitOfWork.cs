using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Infrastructure.Data;

namespace OnlineLearningPlatform.Infrastructure.Repositories
{
    public class UnitOfWork(AppDbContext db) : IUnitOfWork
    {
        private readonly AppDbContext _db = db;

        public IUserProfileRepository UserProfile { get; private set; } = new UserProfileRepository(db);
        public IUserRepository User { get; private set; } = new UserRepository(db);

        // entity repos
        public IAnswerRepository Answer { get; private set; } = new AnswerRepository(db);
        public ICourseRepository Course { get; private set; } = new CourseRepository(db);
        public IEnrollmentRepository Enrollment { get; private set; } = new EnrollmentRepository(db);
        public ILessonRepository Lesson { get; private set; } = new LessonRepository(db);
        public IModuleRepository Module { get; private set; } = new ModuleRepository(db);
        public IProgressRepository Progress { get; private set; } = new ProgressRepository(db);
        public IQuestionRepository Question { get; private set; } = new QuestionRepository(db);
        public IQuizRepository Quiz { get; private set; } = new QuizRepository(db);
        public IQuizSubmissionRepository QuizSubmission { get; private set; } = new QuizSubmissionRepository(db);


        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
