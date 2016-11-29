
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using Java.Lang;
using static MyAttandance.Services.AppLocationService;

namespace MyAttandance.Helper
{
    class BindingHelper<T, X> : Object, IServiceConnection
    {

        private static string LOG_TAG = typeof(BindingHelper<T, X>).Name;
        public IBinder binders;
        public IBindingStatus mCaller;
        public Location loc = null;
        public bool stat = false;

        public interface IBindingStatus
        {
            void isBinded(bool status);
        }
   
        public BindingHelper(IBindingStatus caller)
        {
            mCaller = caller;
        }

        public void bind()
        {
            try
            {
                Application.Context.BindService(new Intent(Application.Context, typeof(X)), this, Bind.NotForeground);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public void unBind()
        {
            try
            {
                Application.Context.UnbindService(this);

            }catch(System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public static bool isMyServiceRunning()
        {
            ActivityManager manager = (ActivityManager)Application.Context.GetSystemService(Context.ActivityService);

            Log.Debug(LOG_TAG, "{0} Info My Service Name ({1})", new string[] { "isMyServiceRunning", typeof(X).Name });

            foreach (ActivityManager.RunningServiceInfo service in manager.GetRunningServices(int.MaxValue))
            {
                Log.Debug(LOG_TAG, "{0} Info service Name ({1})", new string[] { "isMyServiceRunning", service.Service.ClassName });

                if (service.Service.ClassName.Contains(typeof(X).Name))
                {
                    return true;
                }
            }

            return false;
        }

        public void OnServiceConnected(ComponentName name, IBinder binder)
        {
            Log.Debug(LOG_TAG, "{0} Info service Name ({1})", new string[] { "OnServiceConnected", typeof(X).Name });
            binders = binder;
            loc = ((LocalBinder)binders).getLocation();
            stat = stat = ((LocalBinder)binders).getLocStatus();
            mCaller.isBinded(true);
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            Log.Debug(LOG_TAG, "{0} Info service Name ({1})", new string[] { "OnServiceDisconnected", typeof(X).Name });
            mCaller.isBinded(false);
        }



    }
}