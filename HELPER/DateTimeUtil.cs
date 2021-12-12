using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Util;

namespace HELPER
{
    public class DateTimeUtil
    {
        public static Calendar DateTimeToCalendar(DateTime dateTime)
        {
            Calendar calendar = Calendar.GetInstance(Java.Util.TimeZone.GetTimeZone("Asia/Jerusalem"));

            calendar.Set(CalendarField.DayOfMonth,  dateTime.Day);
            calendar.Set(CalendarField.Month,       dateTime.Month - 1);
            calendar.Set(CalendarField.Year,        dateTime.Year);
            calendar.Set(CalendarField.HourOfDay,   dateTime.Hour);
            calendar.Set(CalendarField.Minute,      dateTime.Minute);
            calendar.Set(CalendarField.Second,      dateTime.Second);

            return calendar;
        } 

        public static DateTime CalendarToDateTime(Calendar calendar)
        {
            DateTime dateTime = new DateTime(
                                              calendar.Get(CalendarField.Year),
                                              calendar.Get(CalendarField.Month + 1),
                                              calendar.Get(CalendarField.DayOfMonth),
                                              calendar.Get(CalendarField.HourOfDay),
                                              calendar.Get(CalendarField.Minute),
                                              calendar.Get(CalendarField.Second)
                                             );

            return dateTime;
        }

        public static long CalendarTimeInMillis(DateTime dateTime)
        {
            return DateTimeToCalendar(dateTime).TimeInMillis;
        }

        public static long CalendarTimeInMillis(Calendar calendar)
        {
            return calendar.TimeInMillis;
        }

        public static long ConvertToMillis(int hours, int minutes)
        {
            if (hours == 0)
                return ConvertToMillis(minutes);
            else
                return ConvertToMillis(hours * 60 + minutes);
        }

        public static long ConvertToMillis(int minutes)
        {
            return 1000 * 60 * minutes;
        }

        public static DateTime StringToDateTime(string date, string delimiter = "/")
        {
            string[] d = date.Split(delimiter);

            return new DateTime(Convert.ToInt32(d[2]), Convert.ToInt32(d[1]), Convert.ToInt32(d[0]));
        }

        public static string Age(DateTime birthDate)
        {
            return Age(DateTime.Today, birthDate);
        }

        public static string Age(DateTime from, DateTime to)
        {
            TimeSpan ts = from - to;
            DateTime Age = DateTime.MinValue.AddDays(ts.Days);
            //return string.Format(" {0} Years {1} Month {2} Days", Age.Year - 1, Age.Month - 1, Age.Day - 1));
            return string.Format(" {0}:{1}:{2}", Age.Year - 1, Age.Month - 1, Age.Day - 1);
        }

        public static string DateTimeToSqliteDate(DateTime date)
        {
            return date.Year + "-" + date.Month + "-" + date.Day;
        }

        public static DateTime SqliteDateToDateTime(string date)
        {
            string[] d = null;

            if (date.Contains(' '))
                d = date.Split(" ");

            if (d != null)
                return StringToDateTime(d[0], "-");
            else
                return StringToDateTime(date, "-");
        }

        public static DateTime DateFirstHour(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime DateLastHour(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static bool IsValidDate(string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt);
        }

        /// <summary>
        /// בדיקה אם שעה
        /// </summary>
        /// <param name="time">מחרוזת השעה לבדיקה</param>
        /// <returns>נכון/לא נכון</returns>
        public static bool IsValidTime(string time)
        {
            TimeSpan ts;
            string t;

            if (IsValidDate(time))
                t = time.ToString().Substring(11);  // יש לחלץ את השעה מהתאריך
            else
                t = time.ToString();                //          התקבלה שעה בלבד  

            return (TimeSpan.TryParse(t, out ts));
        }

        /// <summary>
        /// Get the quarter of the year for a given date
        /// </summary>
        /// <param name="date">date to calculate qurter</param>
        /// <returns>Quarter of the year</returns>
        public static int Quarter(DateTime date)
        {
            return (date.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// Get the last date in quarter for a given date
        /// </summary>
        /// <param name="date">Date to calculate end date of quarter</param>
        /// <returns>Last date in quarter</returns>
        public static DateTime QuarterEndDate(DateTime date)
        {
            int quarter = (date.Month - 1) / 3 + 1;
            return new DateTime(date.Year, (quarter * 3) + 1, 1).AddDays(-1);
        }

        /// <summary>
        /// Get the last date in quarter for a given quarter and year
        /// </summary>
        /// <param name="quarter">Quarter to calculate end date of quarter</param>
        /// <param name="year">Year to calculate end date in quarter</param>
        /// <returns>Last date in quarter</returns>
        public static DateTime QuarterEndDate(int quarter, int year)
        {
            return new DateTime(year, (quarter * 3) + 1, 1).AddDays(-1);
        }

        /// <summary>
        /// Get the first date in quarter for a given date
        /// </summary>
        /// <param name="date">Date to calculate first date of quarter</param>
        /// <returns>First date in quarter</returns>
        public static DateTime QuarterStartDate(DateTime date)
        {
            return QuarterEndDate(date).AddDays(1).AddMonths(-3);
        }

        /// <summary>
        /// Get the first date in quarter for a given quarter and year
        /// </summary>
        /// <param name="quarter">Quarter to calculate end date of quarter</param>
        /// <param name="year">Year to calculate end date in quarter</param>
        /// <returns>First date in quarter</returns>
        public static DateTime QuarterStartDate(int quarter, int year)
        {
            return QuarterEndDate(quarter, year).AddDays(1).AddMonths(-3);
        }

        /// <summary>
        /// Get start date and end date of a quarter for a given date
        /// </summary>
        /// <param name="date">Date to calculate first and end dates of quarter</param>
        /// <returns>First and Last date of quater</returns>
        public static DateTime[] QuarterDateRange(DateTime date)
        {
            DateTime[] dates = new DateTime[2];
            dates[0] = QuarterStartDate(date);
            dates[1] = QuarterEndDate(date);
            return dates;
        }

        /// <summary>
        /// Get start date and end date of a quarter for a given quarter and year
        /// </summary>
        /// <param name="quarter">Quarter to calculate</param>
        /// <param name="year">Year to calculate</param>
        /// <returns>First and last dates of quarter</returns>
        public static DateTime[] QuarterDateRange(int quarter, int year)
        {
            DateTime[] dates = new DateTime[2];
            dates[0] = QuarterStartDate(quarter, year);
            dates[1] = QuarterEndDate(quarter, year);
            return dates;
        }

        /// <summary>
        /// בדיקה אם תאריך נמצא בטווח תאריכים
        /// </summary>
        /// <param name="dateToCheck">התאריך לבדיקה</param>
        /// <param name="startDate">תאריך התחלת הטווח</param>
        /// <param name="endDate">תאריך סיום הטווח</param>
        /// <returns>בטווח/איננו בטווח</returns>
        public static bool IsDateInRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            bool isok = startDate.Date <= dateToCheck.Date && dateToCheck.Date <= endDate.Date;
            return startDate <= dateToCheck && dateToCheck <= endDate;
        }
    }
}