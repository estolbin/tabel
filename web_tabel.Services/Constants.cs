namespace web_tabel.Services
{
    public static class Constants
    {
        public const string AUTH_COOKIE_NAME = ".web_table.AuthCookie";
        public const string UNAUTHORIZED_PATH = "/Error/Unauthorized";
        public const string FORBIDDEN_PATH = "/Error/Forbidden";
        
        public const string ADMNIM_ROLE = "Admin";
        public const string PAYROLL_ROLE = "Payroll";
        public const string TIMEKEEPER_ROLE = "Timekeeper";
        public const string USER_ROLE = "User";
    }
}
