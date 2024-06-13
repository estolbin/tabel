using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web_table.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public async Task<IActionResult> Forbidden()
        {
            return View();
        }
        
        public async Task<IActionResult> Unauthorized()
        {
            return View();
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
