using OnlineLearningPlatform.Application.Common.Utility;
using OnlineLearningPlatform.Application.Requests;
using OnlineLearningPlatform.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineLearningPlatform.UI.Areas.Architect.Controllers
{
    [Area(SD.Role_Architect)]
    [Authorize(Roles = SD.Role_Architect)]
    [Route("Architect/[controller]")]
    public class AppUserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allUsers = (await _userService.GetAllUsersAsync()).Data;
            return View(allUsers);
        }

        [HttpGet("Activity/{userName}")]
        public async Task<IActionResult> Activity(string userName)
        {
            var activity = (await _userService.GetUserActivityAsync(userName)).Data;
            return View(activity);
        }

        [HttpPost("Activate/{userName}")]
        public async Task<IActionResult> ActivateUser(string userName)
        {
            var result = (await _userService.ActivateUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User activated successfully." });
            return BadRequest(new { success = false, message = "User activation failed." });
        }

        [HttpPost("Deactivate/{userName}")]
        public async Task<IActionResult> DeactivateUser(string userName)
        {
            var result = (await _userService.DeactivateUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User deactivated successfully." });
            return BadRequest(new { success = false, message = "User deactivation failed." });
        }

        [HttpPost("Suspend")]
        public async Task<IActionResult> SuspendUser([FromBody] SuspendUserRequest request)
        {
            var result = (await _userService.SuspendUserAsync(request)).Data;
            if (result)
                return Ok(new { success = true, message = "User suspended successfully." });
            return BadRequest(new { success = false, message = "User suspension failed." });
        }

        [HttpPost("Unsuspend")]
        public async Task<IActionResult> UnsuspendUser([FromBody] string userName)
        {
            var result = (await _userService.UnsuspendUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User unsuspended successfully." });
            return BadRequest(new { success = false, message = "User unsuspension failed." });
        }

        [HttpPost("Unlock/{userName}")]
        public async Task<IActionResult> UnlockUser(string userName)
        {
            var result = (await _userService.UnlockUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User unlocked successfully." });
            return BadRequest(new { success = false, message = "User unlocking failed." });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _userService.ResetUserPasswordAsync(request);
            if (result.Data)
                return Ok(new { success = true, message = "Password reset successfully." });
            return BadRequest(new { success = false, message = result.Message });
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var result = await _userService.AssignRoleAsync(request);
            if (result.Data)
                return Ok(new { success = true, message = "Role assigned successfully." });
            return BadRequest(new { success = false, message = result.Message });
        }

        [HttpPost("RemoveRole")]
        public async Task<IActionResult> RemoveRole([FromBody] RemoveRoleRequest request)
        {
            var result = await _userService.RemoveRoleAsync(request);
            if (result.Data)
                return Ok(new { success = true, message = "Role removed successfully." });
            return BadRequest(new { success = false, message = result.Message });
        }


        [HttpPost("Delete/{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var result = (await _userService.DeleteUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User deleted successfully." });
            return BadRequest(new { success = false, message = "User deletion failed." });
        }
    }
}
