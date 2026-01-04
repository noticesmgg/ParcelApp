using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreServiceContracts
{
    internal static class BasicExtensions
    {
        private static DateTime MinDate = new DateTime(2000, 1, 1);
        private static DateTime MinSqlDate = new DateTime(1900, 1, 1);

        public static DateTime GetMinDate(this DateTime dt) => MinDate;
        /// <summary>
        /// Converts to Safe Bool
        /// </summary>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static bool ToSafeBool(this object inputObj)
        {
            return safeBool(inputObj);
        }

        /// <summary>
        /// Converts to Safe Decimal
        /// </summary>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static Decimal ToSafeDecimal(this object inputObj)
        {
            return safeDecimal(inputObj);
        }

        /// <summary>
        /// Converts to Safe String
        /// </summary>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static String ToSafeString(this object inputObj)
        {
            return safeStr(inputObj);
        }

        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            StringBuilder result = new StringBuilder();
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string[] words = str.Split(new char[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                string lowerCaseWord = word.ToLower();
                string pascalCaseWord = textInfo.ToTitleCase(lowerCaseWord);
                result.Append(pascalCaseWord);
            }

            return result.ToString();
        }

        public static double? FromCurrencyToDbl(this string input)
        {
            double result = 0.0;
            // Remove parentheses and handle negative values
            bool isNegative = input.StartsWith("(") && input.EndsWith(")");
            if (isNegative)
            {
                input = input.Trim('(', ')');
            }

            // Remove the dollar sign and any commas
            input = input.Replace("$", "").Replace(",", "");

            // Try to parse the cleaned string to a double
            bool success = double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

            if(!success)
            {
                return null;
            }

            // If the value was negative, negate the result
            if (success && isNegative)
            {
                result = -result;
            }

            return result;
        }

        /// <summary>
        /// To Safe Double
        /// </summary>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static double ToSafeDouble(this object inputObj)
        {
            return safeDbl(inputObj);
        }

        public static double Round(this double value, int decimals = 0)
        {
            return Math.Round(value, decimals);
        }

        public static double ToDouble(this double? dbl)
        {
            return dbl ?? 0.0;
        }
        public static string ToDoubleDBNull(this double? dbl)
        {
            return (dbl != null && dbl.HasValue) ? dbl.Value.ToString() :  "null";
        }

        public static string ToIntDBNull(this int? dbl)
        {
            return (dbl != null && dbl.HasValue) ? dbl.Value.ToString() : "null";
        }

        public static double? ToDoubleWithNull(this object dbl)
        {
            return dbl == DBNull.Value ? (double?)null :  (String.IsNullOrEmpty(dbl.ToSafeString()) ? ((double?)null) : dbl.ToSafeDouble());
        }

        public static int? ToIntWithNull(this object i, int defaultValue = 0)
        {
            if (i == null || i == DBNull.Value)
                return null;
            var str = i.ToSafeString();
            if (string.IsNullOrEmpty(str))
                return null;
            return i.ToSafeInt();
        }

        public static string ToSafeDoubleStrWithNull(this object dbl, int decimals = 2)
        {
            var safeDbl = dbl.ToDoubleWithNull();
            return safeDbl.HasValue ? safeDbl.Value.Round(decimals).ToString() : "";
        }

        public static string ToFormattedDoubleString(this double value)
        {
            return value.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static int? ToIntWithNull(this object i)
        {
            return i == DBNull.Value ? (int?)null : i.ToSafeInt();
        }

        public static decimal? ToDecimalWithNull(this object i)
        {
            return i == DBNull.Value ? (decimal?)null : i.ToSafeDecimal();
        }

        public static object ToDBNull(this double? dbl)
        {
            return (dbl != null && dbl.HasValue) ? dbl.Value : DBNull.Value;
        }
        public static object ToDBNull(this double dbl)
        {
            return (dbl == 0) ? DBNull.Value : dbl;
        }
        public static object ToDBNull(this string? str)
        {
            return (str != null ) ? str : DBNull.Value;
        }

        public static object ToDBNull(this DateTime? str)
        {
            return (str != null) ? str : DBNull.Value;
        }

        /// <summary>
        /// Converts to Safe Int
        /// </summary>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static int ToSafeInt(this object inputObj)
        {
            return safeInt(inputObj);

        }

        /// <summary>
        /// Converts to Safe Long
        /// </summary>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static long ToSafeLong(this object inputObj)
        {
            return safeLong(inputObj);
        }

        /// <summary>
        /// Converts to Safe Date
        /// </summary>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static DateTime ToSafeDate(this object inputObj)
        {
            return safeDate(inputObj);
        }

        /// <summary>
        /// Converts to Safe Date
        /// </summary>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static DateTime ToSafeDateWithNoTimeZone(this object inputObj)
        {
            return safeDate(inputObj,true);
        }

        public static DateTime? ToSafeMinNullDate(this object inputObj)
        {
             var dt = safeDate(inputObj);
             if (dt == DateTime.MinValue || dt <= MinSqlDate)
                return null;
             else
                 return dt;
        }

        public static String? ToSafeStringWithNull(this object inputObj)
        {
            if(inputObj == null || inputObj == DBNull.Value)
                return null;
            return  safeStr(inputObj);
        }


        /// <summary>
        /// Converts to Date for DB Format
        /// </summary>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public static String ToDateForDBFormatInclNull(this DateTime? inputDate)
        {
            String result = "";

            if(inputDate == null || !inputDate.HasValue)
                return "null";

            if (DateTime.MinValue.Equals(inputDate) || new DateTime(1900, 1, 1).Equals(inputDate.Value) || (inputDate.Value.Year > 2500))
                result = "null";
            else
                result = inputDate.Value.ToString("yyyy-MM-dd HH:mm:ss").ToSQ();

            return result;
        }

        /// <summary>
        /// Converts Date for DB Format
        /// </summary>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public static String ToDateForDBFormat(this DateTime inputDate)
        {
            String result = "";

            if (DateTime.MinValue.Equals(inputDate) || MinSqlDate.Equals(inputDate))
                result = inputDate.Year + "-" + inputDate.Month + "-" + inputDate.Day;
            else
                result = inputDate.ToString("yyyy-MM-dd");

            return result;
        }

        /// <summary>
        /// Extension Method to check ignoring the case
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="compareObj"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this object inputObj, object compareObj)
        {
            return equalsIgnoreCase(inputObj, compareObj);
        }

        /// <summary>
        /// Gets DateTime For DBFormat
        /// </summary>
        /// <param name="inputDate">Inputs Date</param>
        /// <returns>String</returns>
        public static String ToDateTimeForDBFormat(this DateTime inputDate)
        {
            String result = "";

            //    'YYYY-MM-DD HH:MM:SS'
            result = inputDate.Year + "-" + inputDate.Month + "-" + inputDate.Day;
            result += " " + inputDate.Hour + ":" + inputDate.Minute + ":" + inputDate.Second;
            return result;
        }

        public static DateTime ToDateTime(this object value)
        {
            DateTime convertedDate = new DateTime(1900, 1, 1);
            try
            {
                if (DateTime.TryParse(value.ToSafeString(), out DateTime parsedDate))
                    convertedDate = parsedDate;
            }

            catch
            {

            }

            return convertedDate;
        }

        public static bool ContainsIgnoreCase(this object inputObj, object CompareObj)
        {
            return containsIgnoreCase(inputObj, CompareObj);
        }

        public static String ReplaceIgnoreCase(this String inputObj, String CompareObj, String ReplaceObj)
        {
            return Regex.Replace(inputObj, CompareObj, ReplaceObj, RegexOptions.IgnoreCase).Trim();
        }

        private static bool containsIgnoreCase(object inputObj, object CompareString)
        {
            try
            {
                if (inputObj is string && CompareString is string)
                    return inputObj.ToSafeString().Trim().ToUpper().Contains(CompareString.ToSafeString().Trim().ToUpper());

                if (inputObj == null || CompareString == null)
                    return false;

                String tryThis = Convert.ToString(inputObj);
                String tryThisCompare = Convert.ToString(CompareString);

                return tryThis.ToSafeString().Trim().ToUpper().Contains(tryThisCompare.ToSafeString().Trim().ToUpper());
            }
            catch
            {
                return false;
            }
        }

        private static bool equalsIgnoreCase(object inputObj,object CompareString)
        {
            try
            {
                if (inputObj is string && CompareString is string)
                    return inputObj.ToSafeString().Trim().Equals(CompareString.ToSafeString().Trim(), StringComparison.CurrentCultureIgnoreCase);

                if (inputObj == null || CompareString == null)
                    return false;

                String tryThis = Convert.ToString(inputObj);
                String tryThisCompare = Convert.ToString(CompareString);

                return tryThis.ToSafeString().Trim().Equals(tryThisCompare.ToSafeString().Trim(), StringComparison.CurrentCultureIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static bool safeBool(object inputObj)
        {
            if (inputObj == null)
                return false;

            if (inputObj is bool)
            {
                return (bool)inputObj;
            }

            bool data = false;

            try
            {
                if (safeInt(inputObj) == 1)
                {
                    return true;
                }

                var str = inputObj.ToSafeString().Trim();
                if (str.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else if (str.Equals("No", StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }

                Boolean.TryParse(str, out data);
            }
            catch
            {
                return data;
            }


            return data;
        }

        private static int safeInt(object inputObj)
        {
            try
            {
                if (inputObj is int)
                    return (int)inputObj;
                if (inputObj == null)
                    return 0;

                String tryThis = Convert.ToString(inputObj);

                if (tryThis.Length == 0)
                    return 0;

                tryThis = tryThis.Replace(",", "");

                if (tryThis.Contains("."))
                    return Convert.ToInt32(tryThis.ToSafeDecimal());
                else
                {
                    int result;
                    if (Int32.TryParse(tryThis, out result))
                        return result;
                    else
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        private static long safeLong(object inputObj)
        {
            try
            {
                if (inputObj is long) return (long)inputObj;
                if (inputObj == null)
                    return 0;

                String tryThis = Convert.ToString(inputObj);

                if (tryThis.Length == 0)
                    return 0;

                return Convert.ToInt64(tryThis);
            }
            catch
            {
                return 0;
            }
        }

        private static String safeStr(object inputObj)
        {
            try
            {
                if (inputObj is string) return (string)inputObj;
                if (inputObj == null)
                    return "";

                String tryThis = Convert.ToString(inputObj);

                return tryThis;
            }
            catch
            {
                return "";
            }

        }

        private static double safeDbl(object inputObj)
        {
            try
            {
                if (inputObj is double)
                {
                    var d = (double)inputObj;

                    if (Double.IsInfinity(d) || Double.IsNaN(d))
                        return 0.0;
                    else
                        return d;
                }
                if (inputObj == null)
                    return 0.0;

                String tryThis = Convert.ToString(inputObj);
                if (tryThis.Equals("NaN") || tryThis.Contains("Infinity"))
                    return 0.0;

                if (tryThis.Length == 0)
                    return 0.0;

                tryThis = tryThis.Trim();
                tryThis = tryThis.Replace(",", "");
                if (tryThis.Contains("(") && tryThis.Contains(")"))
                    tryThis = "-" + tryThis.Replace("(", "").Replace(")", "");
                if (tryThis.Contains("%"))
                    tryThis = tryThis.Replace("%", "");

                double dbl = 0.0;
                return (Double.TryParse(tryThis, out dbl) ? dbl : 0.0);
            }
            catch
            {
                return 0.0;
            }
        }

        private static Decimal safeDecimal(object inputObj)
        {
            decimal data = 0;
            var numberStyle = NumberStyles.Number | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowExponent;
            var culture = CultureInfo.CurrentCulture;
            Decimal.TryParse((inputObj ?? String.Empty).ToString(), numberStyle, culture, out data);
            return data;
        }

        private static DateTime safeDate(object inputObj, bool TimeZone = false)
        {
            DateTime DummyDate = DateTime.MinValue;
            try
            {
                if (inputObj is DateTime) return (DateTime)inputObj;
                if (inputObj == null) // || inputObj == DBNull)
                    return DummyDate;

                if (safeStr(inputObj).Trim().Length == 0)
                    return DummyDate;

                var inputString = inputObj.ToString();

                if (TimeZone)
                {
                    if (inputString.Contains("T"))
                        inputString = inputObj.ToString().Split('T')[0];
                }

                DateTime tryThis = Convert.ToDateTime(inputString);
                
                return tryThis;
            }
            catch
            {
                return DummyDate;
            }
        }

        public static string ToSQ(this object inputString, bool checkDateRange = false)
        {
            return sq(inputString, checkDateRange);
        }

        private static string sq(object obj, bool checkDateRange)
        {
            if (obj is string)
            {
                string inputstring = obj.ToString();
                inputstring = escapeStr(inputstring);
                return "'" + inputstring + "'";
            }

            else if (obj is int || obj is byte || obj is long || obj is double || obj is decimal)
            {
                return obj.ToString();
            }

            else if (obj is Boolean)
            {
                return obj.ToSafeBool() ? "1" : "0";
            }
            else if (obj is DateTime)
            {
                if (checkDateRange)
                {
                    DateTime? dt = toRangedDate((DateTime)obj);
                    if (!dt.HasValue)
                        return "''";
                }
                return string.Format("'{0:yyyy-MM-dd HH:MM:ss}'", obj);
            }
            else if (obj == null)
            {
                return "NULL";
            }
            
            return null;
        }

        private static string escapeStr(string inputString)
        {
            inputString = inputString.Replace(@"\", @"\\").Replace("'", @"''").TrimEnd();
            return inputString;
        }

        private static DateTime? toRangedDate(DateTime inputDate)
        {
            // If the date is the minimum value, assume it's uninitialized and treat as null
            if (inputDate == DateTime.MinValue)
                return null;

            // Ensure the date is within the SQL Server valid range
            if (inputDate < new DateTime(1753, 1, 1) || inputDate > new DateTime(9999, 12, 31))
                return null;

            return inputDate;
        }

        public static bool StartsWithIgnoreCase(this String Source, String CompareString)
        {
            if (Source == null)
                Source = String.Empty;

            if (CompareString == null)
                CompareString = String.Empty;

            return Source.Trim().StartsWith(CompareString.Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        public static DateTime AddBusinessDays(this DateTime Date, int delta = 0)
        {
            bool NextDate = false;

            if (delta < 0)
                NextDate = true;

            if (NextDate)
            {
                if (Date.DayOfWeek == DayOfWeek.Sunday)
                    Date = Date.AddDays(1);
                else if (Date.DayOfWeek == DayOfWeek.Saturday)
                    Date = Date.AddDays(2);

            }
            else
            {
                if (delta > 0)
                {
                    if (Date.DayOfWeek == DayOfWeek.Sunday)
                        Date = Date.AddDays(1);
                    else if (Date.DayOfWeek == DayOfWeek.Saturday)
                        Date = Date.AddDays(2);
                }
                else
                {
                    if (Date.DayOfWeek == DayOfWeek.Sunday)
                        Date = Date.AddDays(-2);
                    else if (Date.DayOfWeek == DayOfWeek.Saturday)
                        Date = Date.AddDays(-1);
                }

            }

            while (delta != 0)
            {
                int tick = 0;

                if (delta > 0)
                    tick = 1;
                else
                    tick = -1;

                Date = Date.AddDays(tick);

                while (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    Date = Date.AddDays(tick);
                }

                delta -= tick;
            }

            return Date;
        }

        public static DateTime GetLastMonthEndDate(this DateTime date)
        {
            DateTime now = date;
            DateTime firstDayOfCurrentMonth = new DateTime(now.Year, now.Month, 1);
            return firstDayOfCurrentMonth.AddDays(-1);
        }

        public static DateTime ToSafeMinDate(this object inputObj)
        {
            var date = safeDate(inputObj);
            if (date == DateTime.MinValue)
                return MinDate;
            else
                return date;
        }

        public static bool EndsWithIgnoreCase(this String Source, String CompareString)
        {
            return Source.Trim().EndsWith(CompareString.Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        public static int ToSafeBit(this object inputObj)
        {
            if (inputObj != null && (inputObj.ToSafeString().ContainsIgnoreCase("Yes") || inputObj.ToSafeString().ContainsIgnoreCase("True")))
                return 1;
            else
                return 0;

        }

        /// <summary>
        /// Parses the arguments string into a dictionary
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseArguments(this string arguments)
        {
            var result = new Dictionary<string, string>();
            var args = arguments.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var arg in args)
            {
                var keyValue = arg.Split(new[] { '=' }, 2);
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].TrimStart('-');
                    var value = keyValue[1];
                    result[key] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// Combines the dictionary into an argument string
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string ToArgumentString(this Dictionary<string, object> arguments)
        {
            var result = new StringBuilder();

            foreach (var kvp in arguments)
            {
                result.Append($"-{kvp.Key}={kvp.Value.ToSafeString()} ");
            }

            return result.ToString().TrimEnd();
        }

        /// <summary>
        /// Gets the key from a dictionary based on the value
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? GetKeyFromValue(Dictionary<string, string> dictionary, string value)
        {
            foreach (var kvp in dictionary)
            {
                if (kvp.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        public static DateTime AddBusinessDays(this DateTime date, int delta, HashSet<DateTime> holidays)
        {
            try
            {
                int direction = delta < 0 ? -1 : 1;

                while (delta != 0)
                {
                    date = date.AddDays(direction);
                    if (date.DayOfWeek != DayOfWeek.Saturday &&
                        date.DayOfWeek != DayOfWeek.Sunday &&
                        (holidays == null || !holidays.Contains(date.Date)))
                    {
                        delta -= direction; 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
            }

            return date;
        }
        public static bool? ToSafeBoolWithNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return null;
            var str = obj.ToSafeString();
            if (string.IsNullOrEmpty(str))
                return null;
            return obj.ToSafeBool();
        }
    }
}
