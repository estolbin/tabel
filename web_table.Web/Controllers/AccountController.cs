using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public AccountController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe)
        {
            var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(x => x.Name == username);
            if (user == null) return BadRequest("Пользователь не найден");

            if (user.Password != password) return NotFound("Неверный пароль");

            Response.Cookies.Append(Constants.AUTH_COOKIE_NAME, username, new CookieOptions
            {
                Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1),
                IsEssential = true,
                HttpOnly = true
            });

            return RedirectToAction("Index", "TimeShift");
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete(Constants.AUTH_COOKIE_NAME);
            return RedirectToAction("Index", "TimeShift");
        }
    }
}
