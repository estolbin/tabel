using web_tabel.Services;

namespace web_table.Web.Services
{
    public class CurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccesser;
        private readonly UserService _userService;

        public CurrentUserProvider(
            IHttpContextAccessor httpContextAccessor,
            UserService userService)
        {
            _httpContextAccesser = httpContextAccessor;
            _userService = userService;
        }

        public async Task<string> GetCurrentUser() => 
            _httpContextAccesser.HttpContext.Request.Cookies[Constants.AUTH_COOKIE_NAME];

        public async Task<bool> UserInRole(string role)
        {
            var user = await GetCurrentUser();
            var userInRole = await _userService.IsUserInRole(user, role);
            return !string.IsNullOrEmpty(user) && userInRole;
        }

    }
}
