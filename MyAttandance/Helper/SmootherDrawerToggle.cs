
using Android.App;
using Android.Content.PM;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Support.V7.Widget;
using System.Threading;

namespace MyAttandance.Helper
{
    class SmootherDrawerToggle : ActionBarDrawerToggle
    {
        private ThreadStart runnable;
        private Activity activity;
        private ScreenOrientation orientation = ScreenOrientation.SensorPortrait;

        public SmootherDrawerToggle(Activity activity, DrawerLayout drawerLayout, Toolbar toolbar, int openDrawerContentDescRes, int closeDrawerContentDescRes) : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes)
        {
            this.activity = activity;
        }

        public override void OnDrawerOpened(View drawerView)
        {
            base.OnDrawerOpened(drawerView);
            //invalidateOptionsMenu();
        }

        public override void OnDrawerClosed(View view)
        {
            base.OnDrawerClosed(view);
            //invalidateOptionsMenu();
        }
        
        public override void OnDrawerStateChanged(int newState)
        {
            base.OnDrawerStateChanged(newState);
            if (runnable != null && newState == DrawerLayout.StateIdle)
            {
                new Thread(runnable).Start();

                activity.RequestedOrientation = orientation;
                //((MainActivity)activity).setSelectedMenuItem();
                runnable = null;
            }
        }

        public void runWhenIdle(ThreadStart runnable, ScreenOrientation orientation)
        {
            this.runnable = runnable;
            this.orientation = orientation;
        }
    }
}