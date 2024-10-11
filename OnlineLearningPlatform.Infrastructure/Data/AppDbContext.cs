using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {

        // DbSets for entities
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizSubmission> QuizSubmissions { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Progress> Progresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AppUser configuration
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId);

            // AppUser as Instructor for Courses
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.CoursesTaught)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppUser as Student in Enrollments
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Enrollments)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppUser in QuizSubmissions
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.QuizSubmissions)
                .WithOne(qs => qs.Student)
                .HasForeignKey(qs => qs.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course and other entity mappings
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Modules)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId);

            modelBuilder.Entity<Module>()
                .HasMany(m => m.Lessons)
                .WithOne(l => l.Module)
                .HasForeignKey(l => l.ModuleId);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne(qu => qu.Quiz)
                .HasForeignKey(qu => qu.QuizId);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);

            // Enrollment configuration
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            // Progress configuration
            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Student)
                .WithMany()
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Lesson)
                .WithMany()
                .HasForeignKey(p => p.LessonId);

            // QuizSubmission configuration
            modelBuilder.Entity<QuizSubmission>()
                .HasOne(qs => qs.Quiz)
                .WithMany(q => q.QuizSubmissions)
                .HasForeignKey(qs => qs.QuizId);

            // Define timestamp properties (DateTime and DateTime?)
            modelBuilder.Entity<AppUser>()
                .Property(u => u.DateRegistered)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<AppUser>()
                .Property(u => u.LastLoginDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<AppUser>()
                .Property(u => u.SuspensionEndDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.EnrollmentDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Course>()
                .Property(p => p.CreatedDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Progress>()
                .Property(p => p.CompletionDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<QuizSubmission>()
                .Property(qs => qs.SubmissionDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
