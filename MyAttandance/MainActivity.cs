
using Android.App;
using Android.OS;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using MyAttandance.Helper;
using static MyAttandance.Services.AppLocationService;
using MyAttandance.Services;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using static MyAttandance.ViewHolders.MenuViewHodler;
using MyAttandance.Adapters;
using System.Collections.Generic;
using MyAttendance.Fragments;
using Android.Content.PM;
using System.Threading;
using static MyAttandance.Helper.HttpClientHelper;

namespace MyAttandance
{
    [Activity(Label = "MyAttandance", MainLauncher = true, Theme = "@style/StandardTheme", Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity, IMenuClicked, IHttpClient
    { 
        private V7Toolbar toolbar;
        private NavigationView navigationView;
        private DrawerLayout drawerLayout;
        private ListView listview;
        private static Activity activity;
        private MenuAdapter mAdapter;
        private static Fragment runningFragment = new CheckPointFragment();
        private SmootherDrawerToggle toggle;
        private List<int> frags = new List<int>{ Resource.Drawable.fingerIcon,
                                                 Resource.Drawable.fingerIcon,
                                                 Resource.Drawable.fingerIcon,
                                                 Resource.Drawable.fingerIcon,
                                                 Resource.Drawable.fingerIcon};

        private string[] fragsName = { "Attandance", "Notes", "Daily_Attendance" , "Monthly_Report", "Notes Verify" };
        public static Dictionary<string, string> members = new Dictionary<string, string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            activity = this;

            if (!BindingHelper<LocalBinder, AppLocationService>.isMyServiceRunning())
            {
                StartService(new Intent(this, typeof(AppLocationService)));
            }

            toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);

            navigationView = FindViewById<NavigationView>(Resource.Id.navigation_view);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.navigation_drawer);

            toggle = new SmootherDrawerToggle(this, drawerLayout, toolbar, Resource.String.openDrawer, Resource.String.closeDrawer);
            drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();

            listview = FindViewById<ListView>(Resource.Id.nav_menu);

            mAdapter = new MenuAdapter(ApplicationContext, frags, fragsName, this);
            listview.Adapter = mAdapter;

            populateMemberList();

            //Setting Navigation View Item Selected Listener to handle the item click of the navigation menu
            replaceFragment(runningFragment);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            if(runningFragment.GetType() == typeof(DailyReportFragment) ||
                runningFragment.GetType() == typeof(MonthlyReportFragment) ||
                 runningFragment.GetType() == typeof(NotesVerificationFragment))

                MenuInflater.Inflate(Resource.Menu.withFilters, menu);

            return base.OnPrepareOptionsMenu(menu);
        }

        private void replaceFragment(Fragment frag)
        {
             FragmentManager.BeginTransaction().Replace(Resource.Id.container_frame, frag).AddToBackStack(null).Commit();
        }

        public void runReplaceFragment()
        {
            replaceFragment(runningFragment);
        }

        protected override void OnSaveInstanceState(Bundle bundle)
        {
            base.OnSaveInstanceState(bundle);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void clicked(string button)
        {
            ScreenOrientation orientation = ScreenOrientation.SensorPortrait;

            switch (button)
            {
                case "Attandance": runningFragment = new CheckPointFragment(); orientation = ScreenOrientation.SensorPortrait; break;
                case "Notes": runningFragment = new NotesFragment(); orientation = ScreenOrientation.SensorPortrait; break;
                case "Daily_Attendance": runningFragment = new DailyReportFragment();
                    ((DailyReportFragment)runningFragment).updateUserList(members);
                    orientation = ScreenOrientation.SensorLandscape; break;
                case "Monthly_Report": runningFragment = new MonthlyReportFragment(); orientation = ScreenOrientation.SensorLandscape; break;
                case "Notes Verify": runningFragment = new NotesVerificationFragment(); orientation = ScreenOrientation.SensorLandscape; break;
                case "Logs": break;                   
            }

            //replaceFragment(runningFragment);
            toggle.runWhenIdle(new ThreadStart(runReplaceFragment), orientation);
            drawerLayout.CloseDrawer(Android.Support.V4.View.GravityCompat.Start);
        }

        private void populateMemberList()
        {
            FetchDataAsync(HttpRequestHelper.memberListString(WifiConnectivityHelper.getMacMarshmellow()), this);
        }

        public void AsyncResponse(string response)
        {
            members.Clear();
            members = HttpResponseHelper.responseMemberList(response);

            if(runningFragment is DailyReportFragment) ((DailyReportFragment)runningFragment).updateUserList(members);
        }

    }
}

