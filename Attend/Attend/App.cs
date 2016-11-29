
using System.Collections.Generic;
using System.Diagnostics;
using Attend.ExportedInterface;
using Attend.HelperAndroid;
using Xamarin.Forms;
using static Attend.HelperAndroid.HttpClientHelper;

namespace Attend
{
    public class App : Application, IHttpClient
    {
        public static Dictionary<string, string> members = new Dictionary<string, string>();
        public static string thisDeviceMac = "";

        public interface INotifyOnArrrive
        {
            void receiveMember(Dictionary<string, string> members);
        }

        public App()
        {
            thisDeviceMac = WifiConnectivityHelper.getMacMarshmellow();
            aqcuiredMembers(this);
            MainPage = new SideMenu();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            DependencyService.Get<ILocationObtainer>().isServiceStarted();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            Debug.WriteLine("Hello It's RESUMING");
            // Handle when your app resumes
        }
        public static void aqcuiredMembers(IHttpClient caller)
        {
            FetchDataAsync(HttpRequestHelper.memberListString(
                 WifiConnectivityHelper.getMacMarshmellow()), caller, "Member");
        }

        public void AsyncResponse(string response, string from)
        {
            members = HttpResponseHelper.responseMemberList(response);
        }
    }
}
