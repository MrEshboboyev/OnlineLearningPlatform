using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineLearningPlatform.Application.Common.Interfaces;
using OnlineLearningPlatform.Application.Common.Models;
using OnlineLearningPlatform.Application.DTOs;
using OnlineLearningPlatform.Application.Services.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineLearningPlatform.Infrastructure.Implementations
{
    public class AuthService(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration config,
        IUnitOfWork unitOfWork) : IAuthService
    {
        // inject Identity Managers
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IConfiguration _config = config;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private const int _expirationTokenHours = 12;

        public async Task<ResponseDTO<string>> GenerateJwtToken(AppUser user, IEnumerable<string> roles)
        {
            try
            {
                // create token handler instance
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Key"]);
                var issuer = _config["JwtSettings:Issuer"];
                var audience = _config["JwtSettings:Audience"];

                var claimList = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, user.Id),
                    new(JwtRegisteredClaimNames.Email, user.Email)
                };

                // adding roles to claim List
                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                // signing credentials
                var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    IssuedAt = DateTime.UtcNow,
                    Issuer = issuer,
                    Audience = audience,
                    Expires = DateTime.UtcNow.AddHours(_expirationTokenHours),
                    Subject = new ClaimsIdentity(claimList),
                    SigningCredentials = signingCredentials
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new ResponseDTO<string>(tokenString, "Token generated successfully!");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>(ex.Message);
            }
        }

        public async Task<ResponseDTO<string>> LoginAsync(LoginModel loginModel)
        {
            try
            {
                // Fetching user from the database
                var userFromDb = await _userManager.FindByNameAsync(loginModel.UserName)
                    ?? throw new Exception("User not found!");

                if (!userFromDb.IsActive)
                    throw new Exception("Your account is deactivated. Please contact support.");

                // Check if the account is suspended
                if (userFromDb.IsSuspended)
                {
                    var suspensionEnd = userFromDb.SuspensionEndDate.HasValue
                        ? userFromDb.SuspensionEndDate.Value.ToString("f")
                        : "indefinitely";
                    throw new Exception($"Account is suspended until {suspensionEnd}. Reason: {userFromDb.SuspensionReason}");
                }

                var result = await _signInManager.PasswordSignInAsync(
                    loginModel.UserName,
                    loginModel.Password,
                    isPersistent: false,
                    lockoutOnFailure: true // Enabling lockout on failure
                );

                if (result.IsLockedOut)
                    throw new Exception("Your account is locked due to multiple unsuccessful login attempts. Please try again later.");

                if (!result.Succeeded)
                    throw new Exception("Invalid username or password!");

                // Update last login date
                userFromDb.LastLoginDate = DateTime.Now;
                await _userManager.UpdateAsync(userFromDb);

                // Fetch user roles
                var userRoles = await _userManager.GetRolesAsync(userFromDb);

                // Generate JWT token
                var generatedToken = await GenerateJwtToken(userFromDb, userRoles);

                return new ResponseDTO<string>(generatedToken.Data, "Login successful!");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>($"Login failed: {ex.Message}");
            }
        }

        public async Task<ResponseDTO<string>> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                var emailExist = await EmailExist(registerModel.Email);
                var usernameExist = await UserNameExist(registerModel.UserName);

                if (emailExist.Data || usernameExist.Data)
                    return new ResponseDTO<string>("Username/email already exist!");

                // create new AppUser instance
                AppUser user = new()
                {
                    Email = registerModel.Email,
                    UserName = registerModel.UserName,
                    DateRegistered = DateTime.Now 
                };

                
                // create and added to db user
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if(!result.Succeeded)
                    return new ResponseDTO<string>($"Error : {result.Errors.FirstOrDefault()!.Description}");

                // assign role
                await _userManager.AddToRoleAsync(user, registerModel.Role);

                // create user profile
                UserProfile userProfile = new()
                {
                    Bio = registerModel.Bio,
                    Website = registerModel.Website,
                    UserId = user.Id
                };

                await _unitOfWork.UserProfile.AddAsync(userProfile);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<string>(null, "Registration successful!");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> EmailExist(string email)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(email);

                if (userExist is null)
                    return new ResponseDTO<bool>(false);

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> UserNameExist(string username)
        {
            try
            {
                var userExist = await _userManager.FindByNameAsync(username);

                if (userExist is null)
                    return new ResponseDTO<bool>(false);

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }
    }
}
