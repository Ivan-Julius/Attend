using System;
using System.Globalization;
using System.Diagnostics;

namespace Attend.Helper
{
    public class DateFormatingHelper
    {
        private static string LOG_TAG = typeof(DateFormatingHelper).Name;

        public static string TwelveHoursFormat = "MM/dd/yyyy hh:mm:ss a";
        public static string TwelveHoursFormatFromDB = "MM/dd/yyyy hh:mm:ss tt";
        public static string TwentyFourHoursFormat = "MMM dd, yyyy HH:mm:ss";
        public static string WithDayNameFormat = "ddd, dd MMM yyyy";
        public static string OnlyTimeTwentyFourHoursFormat = "HH:mm:ss";
        public static string LongDateOnlyFormat = "MMM dd, yyyy";
        public static string ShortDateOnlyFormat = "dd-MM-yyyy";
        public static string YearInFrontFormat = "yyyy-MM-dd HH:mm:ss";
        public static string LongDateWithTimeFormat = "MMM dd, yyyy hh:mm:ss";
        public static string ShortDateWithTimeFormat = "dd-MM-yyyy HH:mm:ss";
        public static string remoteSourceDateFormat = "yyyy-MM-dd";

        public static DateTime stringToDateWithFormat(string date, string format)
        {
            return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        }

        public static string getDateOrTime(string input, bool isDate)
        {
            DateTime date = DateTime.Now;
            string result;

            try
            {
                date = DateTime.Parse(input, CultureInfo.InvariantCulture);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            result = date != null ? isDate ? date.ToString(WithDayNameFormat) : date.ToString(OnlyTimeTwentyFourHoursFormat) : "";

            return result;
        }

        public static string secToFullTime(string input)
        {
            string result = "";

            try
            {
                int res = int.Parse(input);
                int hours = res / 3600;
                int minutes = (res % 3600) / 60;
                int seconds = res % 60;

                if (res > 0)
                {
                    result = ((hours > 9) ? hours.ToString() : ('0' + hours.ToString())) +
                            ":" + ((minutes > 9) ? minutes.ToString() : ('0' + minutes.ToString())) +
                            ":" + ((seconds > 9) ? seconds.ToString() : ('0' + seconds.ToString()));
                }
                else
                {
                    result = "00:00:00";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public static string StringToStringWithFormat(string date, string toStringFormat)
        {
            string result = "";

            try
            {
                DateTime res = DateTime.Parse(date, CultureInfo.InvariantCulture);
                result = res.ToString(toStringFormat);

            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public static string stringToTimeElapses(string from, string to)
        {
            double result = 0.0;

            try
            {
                DateTime from_Date = DateTime.Parse(from, CultureInfo.InvariantCulture);
                DateTime to_Date = DateTime.Parse(to, CultureInfo.InvariantCulture);

                result = to_Date.Subtract(from_Date).TotalMilliseconds;

            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return MStoTimeString(result);
        }

        public static string MStoTimeString(double text)
        {
            try
            {
                TimeSpan t = TimeSpan.FromMilliseconds(text);
                return string.Format("{0:D2}:{1:D2}:{2:D2}", (t.Hours > 0) ? t.Hours : 0, (t.Minutes > 0)? t.Minutes : 0, (t.Seconds > 0) ? t.Seconds : 0);

            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Format("{0:D2}:{1:D2}:{2:D2}", 0, 0, 0);
            }
        }

    }
}