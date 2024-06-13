using web_tabel.Services;

namespace web_table.Web.Services
{
    public class AuthorizationErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                if (!context.Request.Path.StartsWithSegments(Constants.FORBIDDEN_PATH))
                {
                    context.Response.Redirect(Constants.FORBIDDEN_PATH);
                }
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                if (!context.Request.Path.StartsWithSegments(Constants.UNAUTHORIZED_PATH))
                {
                    context.Response.Redirect(Constants.UNAUTHORIZED_PATH);
                }
            }
        }
    }
}
