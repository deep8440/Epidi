using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Extensions
{
    public static class DateTimeExtension
    {
        public static long ToUnixEpochDate(this DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        public static DateTimeOffset StartOfWeek(this DateTimeOffset dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return dt.AddDays(-diff);
        }

        public static DateTimeOffset StartOfYear(this DateTimeOffset dt)
        {
            return new DateTimeOffset(new DateTime(dt.Year, 1, 1)).ToOffset(dt.Offset);
        }

        public static DateTimeOffset EndOfYear(this DateTimeOffset dt)
        {
            return new DateTimeOffset(new DateTime(dt.Year, 12, 31, 23, 59, 59)).ToOffset(dt.Offset);
        }

        public static DateTimeOffset DateByDayForWeek(this DateTimeOffset dt, DayOfWeek dayOfWeek, int week)
        {
            DateTimeOffset lastDayOfMonth = new DateTimeOffset(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month), dt.Hour,
                dt.Minute, dt.Second, dt.Offset);

            var lastNeededDayOfMonth = lastDayOfMonth.AddDays(lastDayOfMonth.DayOfWeek < dayOfWeek ?
                dayOfWeek - lastDayOfMonth.DayOfWeek - 7 :
                dayOfWeek - lastDayOfMonth.DayOfWeek);

            return week == 5
                ? lastNeededDayOfMonth
                : lastNeededDayOfMonth.Day <= 28 ? lastNeededDayOfMonth.AddDays(-7 * (4 - week)) : lastNeededDayOfMonth.AddDays(-7 * (5 - week));
        }

        public static DateTimeOffset GetEndOfDay(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, 23, 59, 59, date.Offset);
        }

        public static DateTimeOffset GetStartOfDay(this DateTimeOffset date)
        {
            return date.Date;
        }

        public static DateTime GetStartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime GetEndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
        }

        public static DateTimeOffset GetStartOfMonth(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, date.Offset);
        }

        public static DateTimeOffset GetEndOfMonth(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59, date.Offset);
        }

        public static DateTime GetMiddleOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 15);
        }

        public static DateTimeOffset GetMiddleOfMonth(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, 15, 1, 0, 0, TimeSpan.Zero);
        }

        public static DateTime GetEndOfDayWithoutOffset(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }


        public static DateTimeOffset GetStartOfMinute(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, date.Offset);
        }

        public static DateTimeOffset GetEndOfMinute(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, 59, date.Offset);
        }

        public static DateTimeOffset GetStartOfHour(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, 0, 0, date.Offset);
        }

        public static DateTimeOffset GetEndOfHour(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, 59, 59, date.Offset);
        }

        public static DateTime StartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        public static DateTime EndOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 12, 31);
        }

        public static DateTime GetStartOfHour(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
        }

        public static DateTime GetEndOfHour(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 59, 59);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime GetNextMonthDate(this DateTime dt)
        {
            dt = dt.AddMonths(1);
            return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0);
        }

        public static DateTime GetNextDayDate(this DateTime dt)
        {
            dt = dt.AddDays(1);
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }
    }
}
