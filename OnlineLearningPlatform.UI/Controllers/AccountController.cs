using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OnlineLearningPlatform.Application.Common.Models;
using OnlineLearningPlatform.Application.Services.Interfaces;
using OnlineLearningPlatform.UI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using OnlineLearningPlatform.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineLearningPlatform.UI.Controllers
{
    public class AccountController(IAuthService authService,
        ITokenProvider tokenProvider,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        #region Checking uniqueness methods (Email,UserName)

        [HttpGet]
        public async Task<IActionResult> CheckUsernameEmail(string username, string email)
        {
            bool isUsernameTaken = (await _authService.UserNameExist(username)).Data;
            bool isEmailTaken = (await _authService.EmailExist(email)).Data;

            return Json(new { isUsernameTaken, isEmailTaken });
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error with entered values");
                return View(model);
            }

            var result = await _authService.RegisterAsync(model);

            if (result.Success)
            {
                TempData["success"] = result.Message;
                return RedirectToAction(nameof(Login));
            }

            TempData["error"] = result.Message;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var result = await _authService.LoginAsync(loginModel);

            if (result.Success)
            {
                // sign in user applied
                await SignInUser(result.Data);

                // set token for user
                _tokenProvider.SetToken(result.Data);

                TempData["success"] = "Login successfully!";
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = result.Message;

            return View(loginModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        #region Private Methods
        // Sign In User
        private async Task SignInUser(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            // adding claims
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));


            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        #endregion
    }
}
