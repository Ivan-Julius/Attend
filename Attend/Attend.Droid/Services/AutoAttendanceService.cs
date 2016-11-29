
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;

using Attend.Droid.Helpers;
using Attend.HelperAndroid;

using static Attend.Droid.Helpers.BindingHelper<Attend.Droid.Services.AppLocationService.LocalBinder, Attend.Droid.Services.AppLocationService>;
using static Attend.Droid.Services.AppLocationService;
using static Attend.HelperAndroid.HttpClientHelper;

namespace Attend.Droid.Services
{
    [Service]
    class AutoAttendanceService : Service, IBindingStatus, IHttpClient
    {
        private static string LOG_TAG = "AutoAttendanceService";
        private bool isLocationBinded = false;
        private BindingHelper<LocalBinder, AppLocationService> helpers = null;
        private Location l = null;
        private bool stat = false;
        private string ErrorMessage = "";
        private IBinder mBinder = null;

        public override IBinder OnBind(Intent intent)
        {
            return mBinder;
        }

        public class AttendanceBinder : Binder
        {
            AutoAttendanceService service;

            public AttendanceBinder(AutoAttendanceService service)
            {
                this.service = service;
            }

            public AutoAttendanceService GetDemoService()
            {
                return service;
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            locationServiceBinding(out ErrorMessage);
            
            return StartCommandResult.Sticky;
        }

        public override bool StopService(Intent name)
        {
            locationServiceUnbinding();
            return base.StopService(name);
        }

        private void locationServiceUnbinding()
        {
            if (isLocationBinded) helpers.unBind();
        }

        private void locationServiceBinding(out string msg)
        {
            msg = "Please Restart App";
            helpers = new BindingHelper<LocalBinder, AppLocationService>(this);

            if (isMyServiceRunning())
            {
                System.Diagnostics.Debug.WriteLine("my location service is running");
                if (!isLocationBinded) helpers.bind();
            }
            else msg = "Please Restart App";
        }

        public void isBinded(bool status)
        {
            isLocationBinded = status;

            if(status)
            actionCheckPoint();
        }


        public void actionCheckPoint()
        {
            Log.Debug(LOG_TAG, "Service {0}", "actionCheckPoint");

            string msg = "Button Pressed";
            bool validWifi = false;

            l = (helpers).loc;
            stat = (helpers).stat;

            validWifi = WifiConnectivityHelper.isWifiAvailable();

            if (validWifi) { runCheckPoint(this); }
            else { msg = "Please Turn on Wifi Service"; }

            if (!validWifi)
            {
                NotifyResult(msg);
                StopSelf();
            }
        }

       
        public void AsyncResponse(string response, string from)
        {
            NotifyResult(response);
            StopSelf();
        }

        private static void runCheckPoint(IHttpClient caller)
        {
            FetchDataAsync(
                HttpRequestHelper.checkPointString(
                    WifiConnectivityHelper.getMacMarshmellow(), location.Longitude.ToString(), location.Latitude.ToString()), caller, "Auto Attend");
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