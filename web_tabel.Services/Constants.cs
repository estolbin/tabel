namespace web_tabel.Services
{
    public static class Constants
    {
        public static string AUTH_COOKIE_NAME = ".web_table.AuthCookie";
        public static string UNAUTHORIZED_PATH = "/Error/Unauthorized";
        public static string FORBIDDEN_PATH = "/Error/Forbidden";
        
        public static string ADMNIM_ROLE = "Admin";
        public static string PAYROLL_ROLE = "Payroll";
        public static string TIMEKEEPER_ROLE = "Timekeeper";
        public static string USER_ROLE = "User";
    }
}
