using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.Support.V4.Content;
using Android.Util;
using MyAttandance.Entities;
using MyAttandance.Services;
using MyAttandance.SqlSource;

namespace MyAttandance.Receivers
{
    [BroadcastReceiver]
    [IntentFilter(new[] { ConnectivityManager.ConnectivityAction, WifiManager.WifiStateChangedAction })]

    class WifiReceivers : WakefulBroadcastReceiver
    {
        private string LOG_TAG = "";

        public override void OnReceive(Context context, Intent intent)
        {
            //This also become a clue that this method is a starting point of the whole class
            LOG_TAG = this.GetType().Name;
            Log.Debug("Wifi Receivers", intent.Action.ToString());

            ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;

            if (networkInfo != null)
            {
                string ssid = "";
                string bssid = "";

                if ((networkInfo.Type == ConnectivityType.Wifi) & networkInfo.IsConnected)
                {
                    WifiManager wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
                    WifiInfo wifiInfo = wifiManager.ConnectionInfo;

                    if (wifiInfo != null)
                    {
                        if (wifiInfo.SSID != null && wifiInfo.BSSID != null)
                        {
                            ssid = wifiInfo.SSID; bssid = wifiInfo.BSSID;
                        }
                    }
                }
                
                //Try logging with thread
                ThreadStart logProcess = delegate { keepWifi(context, ssid, bssid, networkInfo.IsConnected.ToString()); };
                Thread thread = new Thread( logProcess );
                thread.Start();

                ThreadStart attendanceProcess = delegate { verifyWifi(intent, ssid, bssid, networkInfo.IsConnected); };
                Thread attendanceProcesser = new Thread(attendanceProcess);
                verifyWifi(intent, ssid, bssid, networkInfo.IsConnected);
            }
        }

        private void keepWifi(Context mContext, string SSID, string BSSID, string status)
        {
            LogDataSource<WifiLog> locLog = new LogDataSource<WifiLog>();

            try
            {
                locLog.Debug(mContext, new List<string> { SSID, BSSID, status });
            }
            catch (System.NullReferenceException ex)
            {
                Log.Debug(LOG_TAG, "{0} news ({1})", new string[] { "setLocationResult", ex.ToString() });
            }
        }

        private void verifyWifi(Intent intent, string SSID, string BSSID, bool status)
        {
            Log.Debug(LOG_TAG, "{0} SSID ({1})", new string[] { "verifyWifi", SSID});
            Log.Debug(LOG_TAG, "{0} BSSID ({1})", new string[] { "verifyWifi", BSSID });
            Log.Debug(LOG_TAG, "{0} Status ({1})", new string[] { "verifyWifi", status.ToString() });
            bool Accepted = false;

            if (Constants.ACCEPTD_SSID.ToLower().Equals(SSID.Trim('"').ToLower()) && Constants.ACCEPTED_BSSID.ToLower().Contains(BSSID.ToLower()))
                Accepted = true;

            Log.Debug(LOG_TAG, "{0} Accepted ({1})", new string[] { "verifyWifi", Accepted.ToString() });

            if (Accepted)
            {
                Intent service = new Intent(Application.Context, typeof (AutoAttendanceService));
                StartWakefulService(Application.Context, service);
            }
           
        }
    }
}