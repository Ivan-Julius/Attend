
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;
using System.Runtime.CompilerServices;
using System.IO;
using Java.Lang;
using Android.Util;
using Android.Gms.Common.Apis;
using Android.Gms.Location;

namespace Attend.Droid.Helpers
{
    public class LocationServiceHelper : Object, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private const string LOG_TAG = "Location Service Helper";
        
         private GoogleApiClient mGoogleApiClient;
         private LocationRequest mLocationRequest;
         private LocationSettingsRequest mLocationSettingsRequest;
         private LocationCallback mLocationCallback;
         private Context mContext;
         //private LogDataSource<LocationLog> locLog;

         private long LOCATION_REQUEST_INTERVAL = 3600000; //1 hour
         private long LOCATION_REQUEST_FASTEST_INTERVAL = 300000; //5 minutes
         private const string LOCATION_KEY = "location_key";
         private const int LOCATION_REQUEST_CHECK_SETTINGS = 0x1;
         private bool isRequestLocationUpdate = false;

         public interface OnLocationServicesHelperListener
         {
             void setLocationResult(Location location);
             void setLocationAvailability(bool state);
         }

         public LocationServiceHelper(Context context, long normal, long fastest)
         {
            mContext = context;
            mLocationCallback = new LocationCallbacks((OnLocationServicesHelperListener) context);
            LOCATION_REQUEST_INTERVAL = normal;
            LOCATION_REQUEST_FASTEST_INTERVAL = fastest;

             //locLog = new LogDataSource<LocationLog>();

            buildGoogleApiClient();
            buildLocationRequest();
            buildLocationSettingsRequest();

             Log.Debug(LOG_TAG, "{0} news : ({1})", new string[] { "OnConnectionFailed", "Created" });
         }

         [MethodImpl(MethodImplOptions.Synchronized)]
         private void buildGoogleApiClient()
         {
             mGoogleApiClient = new GoogleApiClient.Builder(mContext, this, this)
                     .AddApi(LocationServices.API)
                     .Build();
         }

         [MethodImpl(MethodImplOptions.Synchronized)]
         private void buildLocationRequest()
         {
             mLocationRequest = new LocationRequest();
             mLocationRequest.SetInterval(LOCATION_REQUEST_INTERVAL);
             mLocationRequest.SetFastestInterval(LOCATION_REQUEST_FASTEST_INTERVAL);
             mLocationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
         }

         [MethodImpl(MethodImplOptions.Synchronized)]
         private void buildLocationSettingsRequest()
         {

             mLocationSettingsRequest = new LocationSettingsRequest.Builder()
                   .AddLocationRequest(mLocationRequest)
                   .Build();
         }

         public void OnConnected(Bundle bundle)
         {
             const string methodName = "OnConnected";

             LocationAvailability locAvail = LocationServices.FusedLocationApi.GetLocationAvailability(mGoogleApiClient);
             Location location = LocationServices.FusedLocationApi.GetLastLocation(mGoogleApiClient);

             if (location != null)
             {
                 Log.Debug(LOG_TAG, "{0} Lat ({1}) Lon ({2})", new string[] { methodName, location.Latitude.ToString(), location.Longitude.ToString() });
             }

             startLocationUpdates();
         }

         public void OnConnectionSuspended(int i)
         {
             Log.Debug(LOG_TAG, "OnConnectionSuspended Fired "+ i.ToString());
         }

         public void OnConnectionFailed(Android.Gms.Common.ConnectionResult connectionResult)
         {
             string msg = string.Format("error code (%d)", connectionResult.ErrorCode);

             Log.Debug(LOG_TAG, "{0} {1}", new string[] { "OnConnectionFailed", msg });
         }

         public void startLocationUpdates()
         {
             string methodName = "startLocationUpdates()";
             Log.Info(methodName, mGoogleApiClient.IsConnected.ToString());

             if (mGoogleApiClient.IsConnected && !isRequestLocationUpdate)
             {
                 LocationServices.FusedLocationApi.RequestLocationUpdates(mGoogleApiClient, mLocationRequest, mLocationCallback, null);
                 isRequestLocationUpdate = true;

                 Log.Debug(LOG_TAG, "{0} news ({1})", new string[] { methodName, "location update started" });
             }
         }

         public void stopLocationUpdates()
         {
             string methodName = "stopLocationUpdates()";

             if (mGoogleApiClient.IsConnected && isRequestLocationUpdate)
             {
                 LocationServices.FusedLocationApi.RemoveLocationUpdates(mGoogleApiClient, mLocationCallback);
                 isRequestLocationUpdate = false;

                 Log.Debug(LOG_TAG, "{0} news ({1})", new string[] { methodName, "location update stoped" });
             }

             Log.Info(methodName, mGoogleApiClient.IsConnected.ToString());
         }

         public void connect()
         {
             mGoogleApiClient.Connect();
         }

         public void disconnect()
         {
             mGoogleApiClient.Disconnect();
         }


         public void checkLocationSettings()
         {
             //String methodName = "checkLocationSettings()";
             //Log.d(LOG_TAG, String.format("%s: --- start ---", methodName));

             PendingResult result = LocationServices.SettingsApi.CheckLocationSettings( mGoogleApiClient, mLocationSettingsRequest );

             //result.SetResultCallback(this);

             //Log.d(LOG_TAG, String.format("%s: --- end ---", methodName));
         }

         public void OnResult(LocationSettingsResult locationSettingsResult)
         {
             //string methodName = "LocationServices.SettingsApi.onResult()";
             //LogDataSource.debug(mContext, LOG_TAG, methodName, "onResult");

             Statuses status = locationSettingsResult.Status;

             switch (status.StatusCode)
             {
                 case LocationSettingsStatusCodes.Success:
                     //Log.i(LOG_TAG, String.format("%s: All location settings are satisfied.", methodName));

                     //LogDataSource.debug(mContext, LOG_TAG, methodName, "All location settings are satisfied");

                     //mListener.onLocationSettingOk();

                     break;
                 case LocationSettingsStatusCodes.ResolutionRequired:
                     //Log.i(LOG_TAG, String.format("%s: Location settings are not satisfied. Show the user a dialog to upgrade location settings ", methodName));
                     //LogDataSource.debug(mContext, LOG_TAG, methodName, "Location settings are not satisfied");

                     Activity ctx = null;

                     try
                     {
                         ctx = (Activity)mContext;
                     }
                     catch (System.Exception ex)
                     {
                         Log.Debug("OnResult", "Error " + ex.Message);
                        // LogDataSource.debug(mContext, LOG_TAG, methodName, " HIA Service missing ");
                     }

                     if (ctx != null)
                     {
                        try
                         {
                             status.StartResolutionForResult(ctx, LOCATION_REQUEST_CHECK_SETTINGS);
                         }
                         catch (IntentSender.SendIntentException e)
                         {
                             Log.Debug("OnResult", "Error " + e.Message);
                         }

                     }
                     else
                     {
                         //mListener.onLocationSettingOk();
                     }

                     break;
                     case LocationSettingsStatusCodes.SettingsChangeUnavailable:

                     //LogDataSource.debug(mContext, LOG_TAG, methodName, "Location settings are inadequate, and cannot be fixed here.");
                     //Log.i(LOG_TAG, String.format("%s: Location settings are inadequate, and cannot be fixed here. Dialog not created.", methodName));
                     break;
             }

             //Log.d(LOG_TAG, String.format("%s: --- end ---", methodName));
         }




         private class LocationCallbacks : LocationCallback
         {
             private OnLocationServicesHelperListener mListener;

             public LocationCallbacks(OnLocationServicesHelperListener callback)
             {
                 this.mListener = callback;
             }

             public override void OnLocationResult(LocationResult result)
             {
                 base.OnLocationResult(result);
                 //string methodName = "LocationCallback.onLocationResult()";

                 Location location = result.LastLocation;

                 mListener.setLocationResult(location);
             }

             public override void OnLocationAvailability(LocationAvailability locationAvailability)
             {
                 base.OnLocationAvailability(locationAvailability);
                 //string methodName = "LocationCallback.onLocationAvailability()";

                 bool state = locationAvailability.IsLocationAvailable;

                 //Log.Debug(LOG_TAG, "{0} status ({1})", new string[] { methodName, state.ToString() });

                 mListener.setLocationAvailability(state);
             }

         }

        //reverse geocoding
        public void reverse_geoCoding(Location locations)
        {

            IList<Address> addresses = null;
            Geocoder geocoder = new Geocoder(mContext);

            try
            {
                addresses = geocoder.GetFromLocation(locations.Latitude, locations.Longitude, 1);
            }
            catch (IOException ioException)
            {
                // Catch network or other I/O problems.
                //errorMessage = getString(R.string.service_not_available);
                //Log.d(LOG_TAG, "service not available", ioException);
                Log.Debug("reverse_geoCoding", "Error " + ioException.Message);
            }
            catch (IllegalArgumentException e)
            {
                Log.Debug("reverse_geoCoding", "IllegalArgumentException " + e.Message);
                // Catch invalid latitude or longitude values.
                //errorMessage = getString(R.string.invalid_lat_long_used);
                /*og.d(LOG_TAG, "invalid lat long" + ". " +
                        "Latitude = " + locations.getLatitude() +
                        ", Longitude = " +
                        locations.getLongitude(), illegalArgumentException);

                Log.Debug("reverse_geoCoding", "Error " + illegalArgumentException.Message);
            }*/

                // Handle case where no address was found.
                if (addresses == null || addresses.Count == 0)
                {

                    //Log.d(LOG_TAG, "no_address_found");
                    //mListener.onReverseGeodeCodeFinish(false, null);

                }
                else
                {

                    Address address = addresses[0];
                    List<string> addressFragments = new List<string>();

                    for (int i = 0; i < address.MaxAddressLineIndex; i++)
                    {
                        addressFragments.Add(address.GetAddressLine(i));
                    }

                    //mListener.onReverseGeodeCodeFinish(true, addressFragments);
                }
            }
        }

    }
}