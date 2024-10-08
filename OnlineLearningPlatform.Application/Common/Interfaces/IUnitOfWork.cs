namespace OnlineLearningPlatform.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IUserProfileRepository UserProfile { get; }

        // entity repos
        IAnswerRepository Answer { get; }
        ICourseRepository Course { get; }
        IEnrollmentRepository Enrollment { get; }
        ILessonRepository Lesson { get; }
        IModuleRepository Module { get; }
        IProgressRepository Progress { get; }
        IQuestionRepository Question { get; }
        IQuizRepository Quiz { get; }
        IQuizSubmissionRepository QuizSubmission { get; }

        Task SaveAsync();
    }
}
