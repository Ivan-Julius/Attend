
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MyAttandance.ViewHolders
{
    class LocationLogViewHolder : Object
    {
        private static string LOG_TAG = typeof(LocationLogViewHolder).Name;

        private TextView IdStr;
        private TextView DtStr;
        private TextView Lat;
        private TextView Lon;
        private TextView Locstat;

        public void setView(View v)
        {
            IdStr = (TextView)v.FindViewById(Resource.Id.log_id);
            DtStr = (TextView)v.FindViewById(Resource.Id.log_dt_str);
            Lat = (TextView)v.FindViewById(Resource.Id.log_level);
            Lon = (TextView)v.FindViewById(Resource.Id.log_logclass);
            Locstat = (TextView)v.FindViewById(Resource.Id.log_methoda);
        }

        public void setIdStr(string text) { IdStr.Text = text; }
        public void setDtStr(string text) { DtStr.Text = text; }
        public void setLat(string text) { Lat.Text = text; }
        public void setLon(string text)
        {
            string methodName = "setMethods()";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start data : {1}---", methodName, text));
            Lon.Text = text;

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public void setLocstat(string text)
        {
            string methodName = "setMessage()";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start data : {1}---", methodName, text));
            Locstat.Text = text;

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public void setListeners(int viewPosition)
        {
            string methodName = "setListeners()";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start data : {1}---", methodName, viewPosition.ToString()));

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

    }
}