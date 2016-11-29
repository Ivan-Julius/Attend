using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using MyAttandance.Entities;
using MyAttandance.SqlSource;
using static MyAttandance.LocationServiceHelper;

namespace MyAttandance.Services
{
    [Service]
    class AppLocationService : Service, OnLocationServicesHelperListener
    {
        private IBinder mBinder = null;
        public Location location = null;
        public bool isLocationAVailable = false;

        private static LogDataSource<LocationLog> locLog = null;
        private static string LOG_TAG = "LocationService";

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
                return service.location;
            }

            public bool getLocStatus()
            {
                return service.isLocationAVailable;
            }
        }

        public override IBinder OnBind(Intent intent)
        {
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
            locLog = new LogDataSource<LocationLog>();
            LocationServiceHelper LSH = new LocationServiceHelper(this);
            LSH.connect();

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }


        #region interface_Methods
        public void setLocationResult(Location newLocation)
        {
            location = newLocation;

            try
            {
                locLog.Debug(this, new List<string> { location.Latitude.ToString(), location.Longitude.ToString(), isLocationAVailable.ToString() });

            }
            catch (System.NullReferenceException ex)
            {
                Log.Debug(LOG_TAG, "{0} news ({1})", new string[] { "setLocationResult", ex.ToString() });
            }
        }


        public void setLocationAvailability(bool state)
        {
            isLocationAVailable = state;

            try
            {
                locLog.Debug(this, new List<string> { location.Latitude.ToString(), location.Longitude.ToString(), isLocationAVailable.ToString() });
            }
            catch (System.NullReferenceException ex)
            {
                Log.Debug(LOG_TAG, "{0} news ({1})", new string[] { "setLocationAvailability", ex.ToString() });
            }

        }
        #endregion

    }
}