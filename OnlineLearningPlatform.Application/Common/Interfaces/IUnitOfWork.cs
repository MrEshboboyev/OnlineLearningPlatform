namespace OnlineLearningPlatform.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IUserProfileRepository UserProfile { get; }

        Task SaveAsync();
    }
}
