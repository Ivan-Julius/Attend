
using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using Attend.Droid.Helpers;
using Attend.Helper;
using Attend.HelperAndroid;
using static Attend.Droid.Helpers.LocationServiceHelper;
using static Attend.HelperAndroid.HttpClientHelper;

namespace Attend.Droid.Services
{
    [Service]
    class AppLocationService : Service, OnLocationServicesHelperListener, IHttpClient
    {
        private IBinder mBinder = null;
        public static Location location = null;
        public bool isLocationAVailable = false;

        private static string LOG_TAG = "LocationService";

        private const long LOCATION_REQUEST_INTERVAL_ONIMPORTANT = 600000; //10 minutes
        private const long LOCATION_REQUEST_FASTEST_INTERVAL_ONIMPORTANT = 300000; //5 minutes

        private const long LOCATION_REQUEST_INTERVAL_ONNORMAL = 3600000; //1 hour
        private const long LOCATION_REQUEST_FASTEST_INTERVAL_ONNORMAL = 3600000; //1 hour

        private const long LOCATION_REQUEST_INTERVAL_ONAPPONLINE = 10000; //10 sec
        private const long LOCATION_REQUEST_FASTEST_INTERVAL_ONAPPONLINE = 5000; //5 sec

        private const string MorningTimeOfSignificance = "08:30:00";
        private const string AfternoonTimeOfSignificance = "13:30:00";
        private const string EveningTimeOfSignificance = "17:30:00";
        private const string DawnTimeofSignificance = "01:00:00";

        private static DateTime Morning = DateTime.Now;
        private static DateTime Afternoon = DateTime.Now;
        private static DateTime Evening = DateTime.Now;
        private static DateTime Dawn = DateTime.Now;
        private static DateTime current = DateTime.Now;

        private static LocationServiceHelper LSH = null;

        private static bool isFull = false;
        private static bool isMainOnline = false;

        private static int lastState = 0;

        public class LocalBinder : Binder
        {
            AppLocationService service;

            public LocalBinder(AppLocationService service)
            {
                this.service = service;
            }

            public AppLocationService GetDemoService()
            {
                return service;
            }

            public Location getLocation()
            {
                return location;
            }

            public bool getLocStatus()
            {
                return service.isLocationAVailable;
            }         
        }

        public override IBinder OnBind(Intent intent)
        {
            if (!isMainOnline)
            {
                isMainOnline = true;
                if (isMainOnline) MainIsOnline();
            }

            return mBinder;
        }

        public override bool OnUnbind(Intent intent)
        {
            return base.OnUnbind(intent);
        }

        public override void OnCreate()
        {
            mBinder = new LocalBinder(this);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            isMainOnline = isMainActivityRunning();

            Log.Debug("LocationService", "isMainOnline :" + isMainOnline.ToString());

            if (!isMainOnline)
            {
                CheckTime();

            }else
            {
                MainIsOnline();
            }

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void MainIsOnline()
        {
            LSHDisconnection(); LSHConnectionByMain();
        }

        private void CheckTime()
        {
            DateTime current = DateTime.Now;
            string m = current.Date.ToString(DateFormatingHelper.YearInFrontFormat).Replace("00:00:00", MorningTimeOfSignificance);
            string a = current.Date.ToString(DateFormatingHelper.YearInFrontFormat).Replace("00:00:00", AfternoonTimeOfSignificance);
            string e = current.Date.ToString(DateFormatingHelper.YearInFrontFormat).Replace("00:00:00", EveningTimeOfSignificance);
            string d = current.Date.ToString(DateFormatingHelper.YearInFrontFormat).Replace("00:00:00", DawnTimeofSignificance);

            Morning = DateTime.Parse(m, CultureInfo.InvariantCulture);
            Afternoon = DateTime.Parse(a, CultureInfo.InvariantCulture);
            Evening = DateTime.Parse(e, CultureInfo.InvariantCulture);
            Dawn = DateTime.Parse(d, CultureInfo.InvariantCulture);

            if ((Morning <= current && current < Afternoon) || (Evening <= current && current < Dawn))
            {
                if(lastState != 1)
                {
                    lastState = 1;  LSHDisconnection(); LSHConnectionByFirstState();
                }
            }
            else if ((Afternoon <= current && current < Evening) || (Dawn <= current && current < Morning))
            {
                if (lastState != 2)
                {
                    lastState = 2; LSHDisconnection(); LSHConnectionBySecondState();
                }

                if (Dawn <= current && current < Morning) isFull = false;
            }
        }

        private static void LSHDisconnection()
        {
            if (LSH != null)
            {
                LSH.disconnect();
                LSH.Dispose();
            }
        }

        private void LSHConnectionByFirstState()
        {
            LocationServiceHelper LSH = new LocationServiceHelper(this, LOCATION_REQUEST_INTERVAL_ONIMPORTANT, LOCATION_REQUEST_FASTEST_INTERVAL_ONIMPORTANT);
            LSH.connect();
        }

        private void LSHConnectionBySecondState()
        {
            LocationServiceHelper LSH = new LocationServiceHelper(this, LOCATION_REQUEST_INTERVAL_ONNORMAL, LOCATION_REQUEST_FASTEST_INTERVAL_ONNORMAL);
            LSH.connect();
        }

        private void LSHConnectionByMain()
        {
            LocationServiceHelper LSH = new LocationServiceHelper(this, LOCATION_REQUEST_INTERVAL_ONAPPONLINE, LOCATION_REQUEST_FASTEST_INTERVAL_ONAPPONLINE);
            LSH.connect();
        }

        #region interface_Methods
        public void setLocationResult(Location newLocation)
        {
            location = newLocation;
            Log.Debug(LOG_TAG, "{0} news ({1})", new string[] { "setLocationResult", newLocation.Latitude.ToString() });

            isMainOnline = isMainActivityRunning();

            if (!isMainOnline)
            {
                CheckTime();

                if (lastState == 1 && WifiConnectivityHelper.isWifiAvailable())
                {
                    runCheckPoint(this);
                }
            }
        }

        public void setLocationAvailability(bool state)
        {
            isLocationAVailable = state;
        }
        #endregion

        private static void runCheckPoint(IHttpClient caller)
        {
            FetchDataAsync(
                HttpRequestHelper.checkPointString(
                    WifiConnectivityHelper.getMacMarshmellow(), location.Longitude.ToString(), location.Latitude.ToString()), caller, "Auto Attend");
        }

        public void AsyncResponse(string response, string from)
        {
            if (!isFull)
            {
                bool checkfull = bool.Parse((HttpResponseHelper.checkpointResponse(response))[2]);

                if (checkfull)
                {
                    NotifyResult(response);
                    isFull = true;
                }
            }
        }

        private bool isMainActivityRunning()
        {
            ActivityManager activityManager = (ActivityManager)Application.Context.GetSystemService(Context.ActivityService);
            try
            {
                if (Build.VERSION.SdkInt == BuildVersionCodes.M || Build.VERSION.SdkInt == BuildVersionCodes.N)
                {
                    foreach (ActivityManager.AppTask task in activityManager.AppTasks)
                    {
                        Log.Debug(LOG_TAG, "{0} Info task Name ({1})", new string[] { "isMainActivityRunning", task.TaskInfo.BaseActivity.PackageName });
                        if (task.TaskInfo.BaseActivity.PackageName.ToString().Equals(Application.Context.PackageName))
                            return true;
                    }

                }else
                {
                    foreach (ActivityManager.AppTask task in activityManager.AppTasks)
                    {
                        Log.Debug(LOG_TAG, "{0} Info task Name ({1})", new string[] { "isMainActivityRunning", task.TaskInfo.TaskDescription. });
                        //if (task.TaskInfo.OrigActivity.PackageName.ToString().Equals(Application.Context.PackageName))
                        //return true;
                    }
                }
                

            }catch(Exception ex)
            {
                Log.Debug(LOG_TAG, "{0} Error : ({1})", new string[] { "isMainActivityRunning", ex.Message});
            }
            return false;
        }

        public void NotifyResult(string msg)
        {
            Notification.Builder builder = new Notification.Builder(Application.Context)
                  .SetContentTitle("Attend")
                  .SetContentText(msg)
                  .SetSmallIcon(Resource.Drawable.common_plus_signin_btn_icon_dark);
            Notification n = builder.Build();
            NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(0, n);
        }
    }
}