using System.Security.Claims;
using web_tabel.Services;

namespace web_table.Web.Services
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UnitOfWork _unitOfWork;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
            _unitOfWork = new UnitOfWork();
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();

            if (IsPathAllowed(path))
            {
                await _next(context);
                return;
            }

            if(context.Request.Cookies.ContainsKey(Constants.AUTH_COOKIE_NAME))
            {
                var loggedInUser = context.Request.Cookies[Constants.AUTH_COOKIE_NAME];

                var user = _unitOfWork.UserRepository.SingleOrDefault(x => x.Name == loggedInUser);
                string role = user.Role.Name ?? "";
                int userId = user == null ? -1 : user.Id;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, role)
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                context.User = principal;

                await _next(context);
            }
            else
            {
                context.Response.Redirect(Constants.UNAUTHORIZED_PATH);
            }
        }

        private bool IsPathAllowed(string path)
        {
            return path == "/account/login" ||
                   path == "/account/register" ||
                   //path == "/" ||
                   path.StartsWith("/error/");
        }
    }
}
