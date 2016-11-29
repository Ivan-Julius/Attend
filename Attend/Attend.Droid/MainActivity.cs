
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Attend.Droid.Helpers;
using Attend.Droid.Services;
using Plugin.Permissions;
using Xamarin.Forms;
using static Attend.Droid.Services.AppLocationService;

namespace Attend.Droid
{
    [Activity(Label = "Attend", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            UserDialogs.Init(() => (Activity)Forms.Context);

            base.OnCreate(bundle);

            if (!BindingHelper<LocalBinder, AppLocationService>.isMyServiceRunning())
            {
                StartService(new Intent(this, typeof(AppLocationService)));
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);
            DevExpress.Mobile.Forms.Init();

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

