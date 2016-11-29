
using Android.App;
using Android.Content;
using Android.Support.V4.Content;
using MyAttandance.Helper;
using MyAttandance.Services;
using static MyAttandance.Services.AppLocationService;

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