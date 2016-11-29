
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using MyAttandance.Helper;
using static MyAttandance.Helper.BindingHelper<MyAttandance.Services.AppLocationService.LocalBinder, MyAttandance.Services.AppLocationService>;
using static MyAttandance.Helper.HttpClientHelper;
using static MyAttandance.Services.AppLocationService;

namespace MyAttandance.Services
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
            helpers = new BindingHelper<LocalBinder, AppLocationService>(this);
            msg = "Please Restart App";

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

            if (validWifi) { runCheckPoint(); }
            else { msg = "Please Turn on Wifi Service"; }

            if (!validWifi)
            {
                NotifyResult(msg);
                StopSelf();
            }
        }

        private void runCheckPoint()
        {
            FetchDataSync(
                HttpRequestHelper.checkPointString(
                    WifiConnectivityHelper.getMacMarshmellow(), l.Longitude.ToString(), l.Latitude.ToString()), this);
        }

        public void AsyncResponse(string response)
        {
            NotifyResult(response);
            StopSelf();
        }

        public void NotifyResult(string msg)
        {
            Notification.Builder builder = new Notification.Builder(Application.Context)
                 .SetContentTitle("My Attendance")
                 .SetContentText(msg)
                 .SetSmallIcon(Resource.Drawable.fingerIcon);
            Notification n = builder.Build();
            NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(0, n);
        }

    }
}