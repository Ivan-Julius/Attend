
using Android.App;
using Android.Content.PM;
using Android.Views;
using Attend.Droid.ExportedRenderer;
using Attend.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MonthlyReportsPage), typeof(MonthlyReportRenderer))]

namespace Attend.Droid.ExportedRenderer
{
    class MonthlyReportRenderer : PageRenderer
    {
        protected override void OnWindowVisibilityChanged(ViewStates visibility)
        {
            base.OnWindowVisibilityChanged(visibility);

            var activity = (Activity)Context;
            if (visibility == ViewStates.Visible)
            {
                activity.RequestedOrientation = ScreenOrientation.SensorLandscape;
            }
        }
    }
}