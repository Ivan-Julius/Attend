
using Android.App;
using Android.Content.PM;
using Android.Views;
using Attend.Droid.ExportedRenderer;
using Attend.Droid.Helpers;
using Attend.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AttendPage), typeof(AttendRenderer))]

namespace Attend.Droid.ExportedRenderer
{
    class AttendRenderer : PageRenderer
    {
        protected override void OnWindowVisibilityChanged(ViewStates visibility)
        {
            base.OnWindowVisibilityChanged(visibility);

            var metrics = Resources.DisplayMetrics;
            var density = Resources.DisplayMetrics.Density;

            var width = ConversionHelper.GetSize(metrics.WidthPixels, metrics.Xdpi);
            var height = ConversionHelper.GetSize(metrics.HeightPixels, metrics.Ydpi);

            double size = ConversionHelper.GetScreenSizeInInches(width, height);

            var activity = (Activity)Context;
            if (visibility == ViewStates.Visible)
            {
                activity.RequestedOrientation = (size >= 6) ? ScreenOrientation.SensorLandscape : ScreenOrientation.SensorPortrait;
            }
        }
    }
}