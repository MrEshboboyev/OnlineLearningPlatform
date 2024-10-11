using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineLearningPlatform.UI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string Index()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
