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

namespace MyAttandance
{
    class Constants
    {
        public const string SERVER_URL = "http://192.168.12.221/surogates.ashx";
        public const string POST_PARAM_METHOD = "method";
        public const string POST_PARAM_MAC = "mac";
        public const string POST_PARAM_LONGITUDE = "lon";
        public const string POST_PARAM_LATITUDE = "lat";
        public const string POST_PARAM_CHECKPOINT = "3";

        public const string ACCEPTD_SSID = "TKP01";
        public const string ACCEPTED_BSSID = "20:aa:4b:6a:49:ed|20:aa:4b:6a:49:ef|20:aa:4b:6a:62:3b|20:aa:4b:6a:62:3d";

    }
}