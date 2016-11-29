
using Acr.UserDialogs;
using Android.Util;
using Attend.ExportedInterface;
using Attend.HelperAndroid;
using Xamarin.Forms;
using static Attend.HelperAndroid.HttpClientHelper;

namespace Attend.Pages
{
    public class AttendPage : ContentPage, IHttpClient
    {
        private static string LOG_TAG = typeof(AttendPage).ToString();

        public Button pushMeDown;
        public Label Announcer;
        public Image attendButton;
        private double latitude = 0.0;
        private double longitude = 0.0;

        public AttendPage()
        {
            DependencyService.Get<ILocationObtainer>().bindLocationService();
            pushMeDown = new Button
            {
                Text = "Press Me"        
            };

            Announcer = new Label
            {
                Text = "Hello Page"
            };

            pushMeDown.Clicked += delegate
            {
                OnPushedDown();
            };

            Content = new StackLayout
            {
                Children = {
                    Announcer,
                    pushMeDown 
                }
            };
        }

        private void OnPushedDown()
        {
            DependencyService.Get<ILocationObtainer>().locationAvailablity(out latitude, out longitude);

            FetchDataAsync(
                HttpRequestHelper.checkPointString(
                    WifiConnectivityHelper.getMacMarshmellow(), longitude.ToString(), latitude.ToString()), this, "Attend");
        }

        public void AsyncResponse(string response, string from)
        {
            Log.Debug(LOG_TAG, "{0} Response : ({1})", new string[] { "Check Point Response", response });

            if (Device.OS == TargetPlatform.Android)
            {
                Log.Debug(LOG_TAG, "{0} is Device Recognize : ({1})", new string[] { "Check Point Response", "YES" });
            }

            ToastConfig tcg = new ToastConfig(response);
            tcg.Duration = new System.TimeSpan(0, 0, 0, 0, 5000);

            UserDialogs.Instance.Toast(tcg);
        }

    }
}
