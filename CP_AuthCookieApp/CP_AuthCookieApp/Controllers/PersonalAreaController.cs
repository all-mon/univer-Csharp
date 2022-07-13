using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP_AuthCookieApp.Controllers
{
    [Authorize]
    public class PersonalAreaController : Controller
    {
        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
