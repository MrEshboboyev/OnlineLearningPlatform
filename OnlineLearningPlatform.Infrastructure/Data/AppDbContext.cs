using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) :
        IdentityDbContext<AppUser>(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important for IdentityDbContext

            // USER ENTITY
            modelBuilder.Entity<AppUser>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.UserProfile)    // One-to-One with UserProfile
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId);

            // USERPROFILE ENTITY
            modelBuilder.Entity<UserProfile>()
                .HasKey(up => up.Id);

            
            // Timestamps using CURRENT_TIMESTAMP
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
        }
    }
}
