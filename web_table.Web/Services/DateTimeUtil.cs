namespace web_table.Web.Services
{
    public static class DateTimeUtil
    {
        public static bool IsEmpty(this DateTime dateTime)
        {
            return dateTime == default(DateTime);
        }
    }
}
