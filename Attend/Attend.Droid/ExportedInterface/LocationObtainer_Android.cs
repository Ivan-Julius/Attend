
using Attend.ExportedInterface;
using Attend.Droid.ExportedInterface;
using Attend.Droid.Helpers;
using Attend.Droid.Services;
using Xamarin.Forms;
using static Attend.Droid.Services.AppLocationService;
using static Attend.Droid.Helpers.BindingHelper<Attend.Droid.Services.AppLocationService.LocalBinder, Attend.Droid.Services.AppLocationService>;
using Android.Locations;
using Android.Util;
using System.Diagnostics;

[assembly : Dependency (typeof (LocationObtainer_Android))]

namespace Attend.Droid.ExportedInterface
{
    class LocationObtainer_Android : ILocationObtainer, IBindingStatus
    {
        public static bool isServiceBinded = false;
        public static BindingHelper<LocalBinder, AppLocationService> bindingHelpers = null;

        public void bindLocationService()
        {
            Log.Debug("locationObtainer", "trying to bind");

            if (bindingHelpers == null)
            bindingHelpers = new BindingHelper<LocalBinder, AppLocationService>(this);

            if(bindingHelpers != null && !isServiceBinded)
            bindingHelpers.bind();
        }

        public void isServiceStarted()
        {
            if (!isMyServiceRunning())
            {
                startService();
            }
        }

        public void isBinded(bool status)
        {
            isServiceBinded = status;
        }

        public bool locationAvailablity(out double latitude, out double longitude)
        {
            latitude = 0.0;
            longitude = 0.0;
            bool result = false;

            Debug.WriteLine("locationObtainer hello trying to get");

            if (isServiceBinded)
            {
                Debug.WriteLine("locationObtainer serviceBinded");

                if (bindingHelpers.binders != null)
                {
                    Debug.WriteLine("locationObtainer binding helpers no null");

                    if (bindingHelpers.binders.IsBinderAlive)
                    {
                        Debug.WriteLine("locationObtainer binder is Alive");

                        Location l = ((LocalBinder)bindingHelpers.binders).getLocation();
                        result = ((LocalBinder)bindingHelpers.binders).getLocStatus();

                        latitude = (l != null)? l.Latitude : 0;
                        longitude = (l != null)? l.Longitude : 0;

                        Debug.WriteLine("locationObtainer "+ latitude.ToString());
                    }
                }
            }

            return result;
        }
    }
}