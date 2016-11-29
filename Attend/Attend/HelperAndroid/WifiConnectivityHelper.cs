using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.Util;
using Java.Net;
using Java.Util;

namespace Attend.HelperAndroid
{
    public class WifiConnectivityHelper
    {
        private static ConnectivityManager cm;
        private static WifiManager wm;
        private static NetworkInfo ni;

        /*
        public string getDeviceMac()
        {

            string deviceMac = "00:00:00:00:00:00";
            string SHARED_KEY = "DEVICE_MAC";

            String prefDeviceMac = mSharedPreferences.getString(SHARED_KEY, deviceMac);

            if (prefDeviceMac.equalsIgnoreCase(deviceMac))
            {
                WifiManager wifiManager = getWifiManager();
                if (wifiManager != null && wifiManager.isWifiEnabled())
                {
                    deviceMac = wifiManager.getConnectionInfo().getMacAddress();
                    mSharedPreferences.edit().putString(SHARED_KEY, deviceMac).apply();
                }
            }
            else
            {
                deviceMac = prefDeviceMac;
            }

            if (deviceMac.equalsIgnoreCase("02:00:00:00:00:00"))
            {
                deviceMac = getMacExtraWay();
            }

            return deviceMac;
        }
        */

        public static string getMacMarshmellow()
        {
            string result = "00:00:00:00:00:00";

            try
            {
                string interfaceName = "wlan0";
                var interfaces = Collections.List(NetworkInterface.NetworkInterfaces);

                foreach (NetworkInterface intf in interfaces)
                {
                    if (!intf.Name.ToLower().Equals(interfaceName.ToLower()))
                    {
                        continue;
                    }

                    byte[] mac = intf.GetHardwareAddress();

                    //Log.Debug("getMacMarshmellow", mac.ToString());
                    if (mac != null)
                    { 
                        StringBuilder buf = new StringBuilder();
                        foreach (byte aMac in mac)
                        {
                            buf.AppendFormat("{0:x2}:", aMac);
                        }
                        if (buf.Length > 0)
                        {
                            buf.Remove(buf.Length - 1, 1);
                        }

                        Log.Debug("getMacMarshmellow", buf.ToString());
                        result = (buf.ToString()).ToLower();
                    }
                }
            }
            catch (Exception exp)
            {
                Log.Debug("getMacMarshmellow", exp.Message);
            }

            return result;
        }

        private static ConnectivityManager getConnectivityManager()
        {
            if (cm == null)
            {
               // Console.WriteLine("hello from cm null");

                cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            }

            return cm;
        }

        private static NetworkInfo getNetworkInfo()
        {
            if (ni == null)
            {
                //Console.WriteLine("hello from ni null");
                getConnectivityManager();

                if (cm != null)
                {
                    ni = cm.ActiveNetworkInfo;
                }
            }

            return ni;
        }

        private static WifiManager getWifiManager()
        {
            if (wm == null)
            {
                getNetworkInfo();
                if (ni != null && ni.Type == ConnectivityType.Wifi)
                {
                    wm = (WifiManager)Application.Context.GetSystemService(Context.WifiService);
                }
            }

            return wm;
        }

        public static bool isWifiAvailable()
        {
            bool isWifi = false;
         
            WifiManager wm = getWifiManager();
            if(wm != null)
            {
                //Console.WriteLine ("hello from wm");
             
                isWifi = wm.IsWifiEnabled;
            }

            return isWifi;
        }

        

    }
}