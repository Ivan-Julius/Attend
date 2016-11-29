
using Android.App;
using Android.Content;
using Android.Support.V4.Content;
using Attend.Droid.Services;
using Attend.Droid.Helpers;
using static Attend.Droid.Services.AppLocationService;

namespace MyAttandance.Receivers
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]

    class OnBootStartReceiver : WakefulBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (!BindingHelper<LocalBinder, AppLocationService>.isMyServiceRunning())
            {
                Intent service = new Intent(Application.Context, typeof(AppLocationService));
                StartWakefulService(Application.Context, service);
            }
        }
    }
}