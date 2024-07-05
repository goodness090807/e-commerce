namespace e_commerce.Common.Utils
{
    public static class DateTimeHelper
    {
        public static DateTime GetUTC8Now()
        {
            return DateTimeOffset.UtcNow.DateTime.AddHours(8);
        }
    }
}
