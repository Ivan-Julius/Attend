
using Android.App;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.Locations;
using Android.Content;

using MyAttandance.Helper;
using MyAttandance.Services;

using static MyAttandance.Helper.HttpClientHelper;
using static MyAttandance.Services.AppLocationService;
using static MyAttandance.Helper.BindingHelper<MyAttandance.Services.AppLocationService.LocalBinder, MyAttandance.Services.AppLocationService>;
using MyAttandance;

namespace MyAttendance.Fragments
{
    public class CheckPointFragment : Fragment, IBindingStatus, IHttpClient
    {
        private string ErrorMessage = "";
        private bool isLocationBinded = false;
        private BindingHelper<LocalBinder, AppLocationService> helpers = null;
        private Location l = null;
        private bool stat = false;
        private Context mContext;

        public override void OnCreate(Bundle savedInstanceState)
        {
            locationServiceBinding(out ErrorMessage);
            mContext = Application.Context;
            base.OnCreate(savedInstanceState);
        }

        public override void OnDestroy()
        {
            locationServiceUnbinding();
            base.OnDestroy();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_checkPoint, container, false);

            ImageButton button = (ImageButton)rootView.FindViewById(Resource.Id.button_checkPoint);

            button.Click += delegate
            {
                actionCheckPoint();
            };

            Animation animation = AnimationUtils.LoadAnimation(mContext, Resource.Animation.rotate);
            animation.Reset();

            ImageView imageView = (ImageView)rootView.FindViewById(Resource.Id.spinner);
            imageView.ClearAnimation();
            imageView.StartAnimation(animation);

            return rootView;
        }

        public void actionCheckPoint()
        {
            string msg = "Button Pressed";

            bool validLocation = false;
            bool validWifi = false;

            if (isLocationBinded)
            {
                if (helpers.binders != null)
                {
                    if (helpers.binders.IsBinderAlive)
                    {
                        l = ((LocalBinder)helpers.binders).getLocation();
                        stat = ((LocalBinder)helpers.binders).getLocStatus();

                        validLocation = true;
                    }
                    else msg = "Please Restart App";
                }
                else msg = "Please Restart App";
            }
            else msg = ErrorMessage;

            if (validLocation) validWifi = WifiConnectivityHelper.isWifiAvailable();

            if (validWifi) { runCheckPoint(); }
            else { msg = "Please Turn on Wifi Service"; }

            if (!validLocation && !validWifi) Toast.MakeText(mContext, msg, ToastLength.Long).Show();
        }

        private void locationServiceBinding(out string msg)
        {
           helpers = new BindingHelper<LocalBinder, AppLocationService>(this);
           msg = "Please Restart App";

            if (isMyServiceRunning())
            {
                System.Diagnostics.Debug.WriteLine("my service is running");
                if (!isLocationBinded) helpers.bind();
            }
            else msg = "Please Restart App";
        }

        private void locationServiceUnbinding()
        {
            if (isLocationBinded) helpers.unBind();
        }

        public void isBinded(bool status)
        {
            isLocationBinded = status;
        }

        private void runCheckPoint()
        {
           FetchDataAsync(
               HttpRequestHelper.checkPointString(
                   WifiConnectivityHelper.getMacMarshmellow(), l.Longitude.ToString(), l.Latitude.ToString()), this);
        }

        public void AsyncResponse(string response)
        {
            Toast.MakeText(mContext, response, ToastLength.Long).Show();
        }
    }
}