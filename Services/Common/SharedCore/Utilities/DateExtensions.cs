using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCore.Utilities
{
    public static class DateExtensions
    {
        /// <summary>
        /// Returns the last day of the previous quarter for the given date
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static DateTime LastQuarterEndDate(this DateTime currentDate)
        {
            int currentQuarter = (currentDate.Month - 1) / 3 + 1;
            DateTime lastQuarterEndDate;

            switch (currentQuarter)
            {
                case 1:
                    // Last quarter is Q4 of the previous year
                    lastQuarterEndDate = new DateTime(currentDate.Year - 1, 12, 31);
                    break;
                case 2:
                    // Last quarter is Q1 of the current year
                    lastQuarterEndDate = new DateTime(currentDate.Year, 3, 31);
                    break;
                case 3:
                    // Last quarter is Q2 of the current year
                    lastQuarterEndDate = new DateTime(currentDate.Year, 6, 30);
                    break;
                case 4:
                    // Last quarter is Q3 of the current year
                    lastQuarterEndDate = new DateTime(currentDate.Year, 9, 30);
                    break;
                default:
                    throw new InvalidOperationException("Invalid quarter calculation.");
            }

            return lastQuarterEndDate;
        }

        public static TimeSpan ToSafeTime(this object inputObj)
        {
            TimeSpan time;

            if (inputObj is TimeSpan)
                return (TimeSpan)inputObj;

            if (!TimeSpan.TryParse(inputObj.ToString(), out time))
            {
                return time;
            }
            else
            {
                return TimeSpan.Parse("00:00");
            }
        }

        /// <summary>
        /// Returns the last day of the previous month for the given date.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns>The last day of the previous month.</returns>
        public static DateTime LastMonthEndDate(this DateTime currentDate)
        {
            // Calculate the first day of the current month
            DateTime firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            // Subtract one day to get the last day of the previous month
            DateTime lastDayOfPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);

            return lastDayOfPreviousMonth;
        }

        public static DateTime MonthEndDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static List<DateTime> GetLastNMonths(this DateTime date, int n)
        {
            var dates = new List<DateTime>();
            for (int i = 0; i < n; i++)
            {
                dates.Add(date.AddMonths(-i).MonthEndDate());
            }
            return dates;
        }

        public static List<DateTime> GetLastNQuarters(this DateTime date, int n)
        {
            var dates = new List<DateTime>();
            for (int i = 0; i < n; i++)
            {
                dates.Add(date.AddMonths(-3 * i).MonthEndDate());
            }
            return dates;
        }

        public static DateTime GetPrevBusinessDay(this DateTime seDate)
        {
            return seDate.AddBusinessDays(-1);
        }

        public static string padMonthForDateFormat(string dateSource)
        {
            if (dateSource.StartsWith("1/") || dateSource.StartsWith("2/") || dateSource.StartsWith("3/") || dateSource.StartsWith("4/") || dateSource.StartsWith("5/") || dateSource.StartsWith("6/")
                || dateSource.StartsWith("7/") || dateSource.StartsWith("8/") || dateSource.StartsWith("9/"))
                dateSource = "0" + dateSource;


            return dateSource.Trim();
        }

        public static string USDateFormat(string dateSource)
        {
            if (string.IsNullOrWhiteSpace(dateSource))
                return string.Empty;

            if (DateTime.TryParse(dateSource, out var dt))
            {
                return dt.ToString("MM/dd/yyyy");
            }

            var parts = dateSource.Split('/');
            if (parts.Length >= 3)
            {
                string month = parts[0].PadLeft(2, '0');
                string day = parts[1].PadLeft(2, '0');
                string year = parts[2].Length > 4 ? parts[2].Substring(0, 4) : parts[2];
                return $"{month}/{day}/{year}";
            }

            return dateSource;
        }


        public static bool isEuropeanDate(System.Data.DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string dateSource = dr.Table.Columns.Contains("TrdDate")
                     ? dr["TrdDate"].ToSafeString()
                     : dr.Table.Columns.Contains("TradeDate")
                         ? dr["TradeDate"].ToSafeString()
                         : string.Empty;

                    if (dateSource.Length == 0)
                        continue;

                    dateSource = dateSource.Replace(" 12:00:00 AM", "");
                    dateSource = dateSource.Replace(" 0:00", "");

                    dateSource = padMonthForDateFormat(dateSource);

                    string firstSlash = dateSource.Substring(2, 1);
                    string secondSlash = dateSource.Substring(5, 1);

                    if (firstSlash.EqualsIgnoreCase("/") && secondSlash.EqualsIgnoreCase("/"))
                    {
                        double firstPortion = dateSource.Substring(0, 2).ToSafeDouble();
                        double secondPortion = dateSource.Substring(3, 2).ToSafeDouble();

                        if (firstPortion > 12)
                            return true;

                        if (secondPortion > 12)
                            return false;
                    }
                }
            }
            catch (Exception)
            {

                return true;
            }


            return true;
        }


    }
}
