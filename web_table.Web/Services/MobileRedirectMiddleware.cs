namespace web_table.Web.Services
{
    public class MobileRedirectMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly string _redirectUrl;

        public MobileRedirectMiddleware(RequestDelegate next, string redirectUrl)
        {
            _next = next;
            _redirectUrl = redirectUrl;
        }

        public async Task Invoke(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString().ToLower();

            if (userAgent.Contains("mobile") && !context.Request.Path.StartsWithSegments("/Mobile"))
            {
                context.Response.Redirect(_redirectUrl);
                return;
            }

            await _next(context);
        }
    }
}
