using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Domain.Entities;
namespace OnlineLearningPlatform.Infrastructure.Data
{
    public class DbInitializer(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        AppDbContext db) : IDbInitializer
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly AppDbContext _db = db;

        public async Task InitializeAsync()
        {
            try
            {
                // Migrate database changes
                if (_db.Database.GetPendingMigrations().Any())
                {
                    await _db.Database.MigrateAsync();
                }

                // Check if the "Admin" role exists, if not, create roles and the admin user
                if (!await _roleManager.RoleExistsAsync(SD.Role_Architect))
                {
                    // Create roles
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Architect));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Instructor));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Student));

                    // Create admin user
                    var adminForDb = new AppUser
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        DateRegistered = DateTime.Now,
                        EmailConfirmed = true,
                        IsActive = true,
                        NormalizedUserName = "ADMIN@EXAMPLE.COM",
                        NormalizedEmail = "ADMIN@EXAMPLE.COM"
                    };

                    await _userManager.CreateAsync(adminForDb, "Admin*123");

                    // Assign role to admin
                    var admin = await _userManager.FindByEmailAsync("admin@example.com")
                        ?? throw new Exception("Admin not found!");
                    await _userManager.AddToRoleAsync(admin, SD.Role_Architect);

                    // Create user profile for admin
                    var userProfile = new UserProfile
                    {
                        Bio = "Admin",
                        UserId = adminForDb.Id,
                        Website = "jobboardapp.com"
                    };

                    await _db.UserProfiles.AddAsync(userProfile);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                throw new Exception("Error initializing the database", ex);
            }
        }
    }

}
