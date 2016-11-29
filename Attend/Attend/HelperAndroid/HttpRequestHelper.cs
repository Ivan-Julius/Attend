
using Android.Util;
using System.Net;
using System.Collections.Generic;
using Java.Security;
using Java.Lang;

namespace Attend.HelperAndroid
{
    public class HttpRequestHelper
    {
        private static string BASE_URL = Constants.SERVER_URL;

        public static string requestMacModifier(string[] RequestMac)
        {
            string result = "'"; int count = 0;
            foreach (string rqMac in RequestMac)
            {
                result = result + rqMac;
                if (count != RequestMac.Length - 1)
                    result = result + "','";
                count++;
            }
            result = result + "'";

            return WebUtility.UrlEncode(result);
        }

        public static string requestTypeModifier(string[] RequestType)
        {
            string result = "";
            foreach (string types in RequestType)
            {
                result = result + types + ",";
            }

            result = (result.Length > 0)? result.Remove(result.Length - 1, 1) : "";
            result.Trim();

            return WebUtility.UrlEncode(result);
        }

        public static string requestStatusModifier(string[] ReqStatus)
        {
            string result = ""; int count = 0;
            if (ReqStatus != null)
            {
                foreach (string stat in ReqStatus)
                {
                    result = result + stat;
                    if (count != ReqStatus.Length - 1)
                        result = result + ",";
                    count++;
                }
            }
            result.Trim();

            return WebUtility.UrlEncode(result);
        }

        private static string hash(string pass)
        {

            string generatedPassword = "";
            try
            {
                // Create MessageDigest instance for MD5
                MessageDigest md = MessageDigest.GetInstance("MD5");
                String pas = (String)pass;

                //Add password bytes to digest
                md.Update(pas.GetBytes());
                //Get the hash's bytes
                byte[] bytes = md.Digest();
                //This bytes[] has bytes in decimal format;
                //Convert it to hexadecimal format
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(Integer.ToString((bytes[i] & 0xff) + 0x100, 16).Substring(1));
                }
                //Get complete hashed password in hex format
                generatedPassword = null;
                generatedPassword = sb.ToString();
            }
            catch (NoSuchAlgorithmException e)
            {
                Log.Debug("hash", string.Format(" Error({0})", e.Message));
            }

            return WebUtility.UrlEncode(generatedPassword);
        }

        public static string checkPointString(string deviceMac, string Longitude, string Latitude)
        {

            Log.Debug("checkPointString", string.Format("deviceMac({0}) long({1}) lat({2})", deviceMac, Longitude, Latitude));

            string uri = BASE_URL+"?";

            uri += Constants.POST_PARAM_METHOD + "=" + Constants.POST_PARAM_CHECKPOINT + "&";
            uri += Constants.POST_PARAM_MAC + "=" + deviceMac + "&";
            uri += Constants.POST_PARAM_LONGITUDE + "=" + Longitude + "&";
            uri += Constants.POST_PARAM_LATITUDE + "=" + Latitude;

            Log.Debug("checkPointString", string.Format("final uri({0})", uri));

            return uri;
        }

        public static string dailyReportString(string deviceMac, string[] deviceMacs, string fdate, string tdate, string[] typeFilter, string[] statusFilter)
        {
            string uri = BASE_URL + "?";

            uri += Constants.POST_PARAM_METHOD + "=6&";
            uri += Constants.POST_PARAM_MAC + "=" + deviceMac + "&";
            uri += "from_date=" + fdate + "&";
            uri += "to_date=" + tdate + "&";
            uri += "mac_list=" + requestMacModifier(deviceMacs) + "&";
            uri += "type_list=" + requestTypeModifier(typeFilter) + "&";
            uri += "status_list=" + requestStatusModifier(statusFilter);

            Log.Debug("dailyReportString", string.Format("final uri({0})", uri));

            return uri;
        }

        public static string monthlyReportString(string deviceMac, string[] deviceMacs, string fdate, string tdate)
        {
            string uri = BASE_URL + "?";

            uri += Constants.POST_PARAM_METHOD + "=7&";
            uri += Constants.POST_PARAM_MAC + "=" + deviceMac + "&";
            uri += "from_date=" + fdate + "&";
            uri += "to_date=" + tdate + "&";
            uri += "mac_list=" + requestMacModifier(deviceMacs);

            Log.Debug("monthlyReportString", string.Format("final uri({0})", uri));

            return uri;
        }

        public static string memberListString(string deviceMac)
        {
            string uri = BASE_URL + "?";

            uri += Constants.POST_PARAM_METHOD + "=10&";
            uri += Constants.POST_PARAM_MAC + "=" + deviceMac;

            Log.Debug("memberListString", string.Format("final uri({0})", uri));

            return uri;
        }

        public static string notesRequestString(string[] RequestMac, string fDate, string tDate, string typeFilter, string statusFilter)
        {
            string uri = BASE_URL + "?";

            uri += Constants.POST_PARAM_METHOD + "=1&";
            uri += "from_date=" + fDate + "&";
            uri += "to_date=" + tDate + "&";
            uri += "mac_list=" + requestMacModifier(RequestMac) + "&";
            uri += "approve=" + statusFilter + "&";
            uri += "reason_type=" + typeFilter;

            Log.Debug("notesRequestString", string.Format("final uri({0})", uri));

            return uri;
        }

        public static string notesSubmissionString(string deviceMac, Dictionary<string, string> noteMap, string Longitude, string Latitude)
        {
            int count = 0;

            string uri = BASE_URL + "?";

            uri += Constants.POST_PARAM_METHOD + "=4&";
            uri += Constants.POST_PARAM_MAC + "=" + deviceMac + "&";
            uri += Constants.POST_PARAM_LONGITUDE + "=" + Longitude + "&";
            uri += Constants.POST_PARAM_LATITUDE + "=" + Latitude + "&";

            foreach (string key in noteMap.Keys)
            {
                uri += key + "="+ noteMap[key];

                count++;

                if (count < noteMap.Count)
                    uri += "&";
            }

            Log.Debug("notesSubmissionString", string.Format("final uri({0})", uri));
          // System.Console.WriteLine(string.Format("final uri ({0})", uri));

            return uri;
        }

        public static string notesConfirmationString(string mac, List<string> id, string approval)
        {
            string uri = BASE_URL + "?";

            string ids = "";
            int c = 0;
            foreach (string i in id)
            {
                ids = ids + i;
                if (c != id.Count - 1)
                    ids = ids + ",";
                c++;
            }

            uri += Constants.POST_PARAM_METHOD + "=2&";
            uri += Constants.POST_PARAM_MAC + "=" + mac + "&";
            uri += "excuse_id=" + ids + "&";
            uri += "approve=" + approval + "&";
            //uri += "pass=" + password;

            Log.Debug("monthlyReportString", string.Format("final uri({0})", uri));

            return uri;
        }


      

    }
}