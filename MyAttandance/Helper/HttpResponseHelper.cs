
using System.Collections.Generic;
using Android.Util;
using MyAttandance.Entities;
using Org.Json;

namespace MyAttandance.Helper
{
    class HttpResponseHelper
    {
        private static string LOG_TAG = typeof(HttpResponseHelper).Name;

        public static List<string> checkpointResponse(string response)
        {
            List<string> result = new List<string>();

            try
            {
                JSONObject cPoint = new JSONObject(response);

                result.Add(cPoint.GetString("firstlogin"));
                result.Add(cPoint.GetString("lastlogin"));
                result.Add(cPoint.GetString("isfull"));
                result.Add(cPoint.GetString("isLate"));
                result.Add(cPoint.GetString("lateCount"));
            }
            catch (JSONException e)
            {
                Log.Debug("checkpointResponse", e.Message);
            }
            return result;
        }

        public static List<Notes> retrieveNotesResponse(string response)
        {
            List<Notes> result = new List<Notes>();

            try
            {
                JSONArray jsonArray = new JSONArray(response);
                int dataCount = jsonArray.Length();
                for (int i = dataCount - 1; i >= 0; i--)
                {
                    JSONObject report = jsonArray.GetJSONObject(i);
                    result.Add(new Notes(
                            report.GetInt("excuse_id"),
                            report.GetString("from_date"),
                            report.GetString("to_date"),
                            report.GetString("excuse_type"),
                            report.GetString("excuse_reason"),
                            report.GetString("approved"),
                            report.GetString("logdate"),
                            report.GetString("owner")));

                    Log.Debug(LOG_TAG, string.Format("{0} JSON i : {1}", "retrieveNotesResponse", report.ToString()));
                }
            }
            catch (JSONException e)
            {
                Log.Debug("reportDailyResponse", e.Message);
            }

            return result;
        }

        public static List<MonthlyReport> reportMonthlyResponse(string response)
        {
            List<MonthlyReport> result = new List<MonthlyReport>();

            try
            {
                JSONArray jsonArray = new JSONArray(response);
                int dataCount = jsonArray.Length();
                for (int i = dataCount - 1; i >= 0; i--)
                {
                    JSONObject report = jsonArray.GetJSONObject(i);
                    result.Add(new MonthlyReport(
                            report.GetString("report_year"),
                            report.GetString("report_month"),
                            report.GetString("name"),
                            report.GetString("count_AllowedLate"),
                            report.GetString("count_earlyLeave"),
                            report.GetString("count_full"),
                            report.GetString("count_late"),
                            report.GetString("count_login"),
                            report.GetString("count_not_full"),
                            report.GetString("count_not_login"),
                            report.GetString("count_sick"),
                            report.GetString("count_leave"),
                            report.GetString("overtime_elapse_time"),
                            report.GetString("total_login")));

                    Log.Debug(LOG_TAG, string.Format("{0} JSON i : {1}", "reportMonthlyResponse", report.ToString()));
                }

            }
            catch (JSONException e)
            {
                Log.Debug("reportDailyResponse", e.Message);
            }

            return result;
        }

        public static List<DailyReport> reportDailyResponse(string response)
        {
            List<DailyReport> result = new List<DailyReport>();

            try
            {
                JSONArray jsonArray = new JSONArray(response);
                int dataCount = jsonArray.Length();
                for (int i = dataCount - 1; i >= 0; i--)
                {
                    JSONObject report = jsonArray.GetJSONObject(i);

                    result.Add(new DailyReport(
                                (i + 1).ToString(),
                                report.GetString("login_elapse_time"),
                                report.GetString("overtime_elapse_time"),
                                report.GetBoolean("is_late"),
                                report.GetBoolean("is_full"),
                                report.GetString("name"),
                                report.GetString("first_login"),
                                report.GetString("last_login"),
                                report.GetString("logout_time"),
                                report.GetString("start_overtime"),
                                report.GetString("login_type")));

                    Log.Debug(LOG_TAG, string.Format("{0} JSON i : {1}", "reportDailyResponse", report.ToString()));
                }
            }
            catch (JSONException e)
            {
                Log.Debug("reportDailyResponse", e.Message);
            }

            return result;
        }

        public static Dictionary<string, string> responseMemberList(string response)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            try
            {
                JSONArray jsonArray = new JSONArray(response);
                int dataCount = jsonArray.Length();
                for (int i = dataCount - 1; i >= 0; i--)
                {
                    JSONObject report = jsonArray.GetJSONObject(i);

                    if(!result.ContainsKey(report.GetString("username")))
                    result.Add(report.GetString("username"), report.GetString("mac"));

                    Log.Debug(LOG_TAG, string.Format("{0} JSON i : {1}", "responseMemberList", report.ToString()));
                }
            }
            catch (JSONException e)
            {
                System.Console.WriteLine(e.Message);
            }

            return result;
        }



    }
}