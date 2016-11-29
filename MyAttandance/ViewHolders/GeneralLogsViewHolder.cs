
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MyAttandance.ViewHolders
{
    class GeneralLogsViewHolder : Object
    {
        private static string LOG_TAG = typeof(GeneralLogsViewHolder).Name;

        //Field Declaration & id pairing MUST have get In-front of name
        private TextView IdStr;
        private TextView DtStr;
        private TextView Level;
        private TextView classes;
        private TextView Methods;
        private TextView Message;

        public void setView(View v)
        {
            IdStr = (TextView)v.FindViewById(Resource.Id.log_id);
            DtStr = (TextView)v.FindViewById(Resource.Id.log_dt_str);
            Level = (TextView)v.FindViewById(Resource.Id.log_level);
            classes = (TextView)v.FindViewById(Resource.Id.log_logclass);
            Methods = (TextView)v.FindViewById(Resource.Id.log_methoda);
            Message = (TextView)v.FindViewById(Resource.Id.log_messages);
        }

        public void setIdStr(string text) { IdStr.Text = text; }
        public void setDtStr(string text) { DtStr.Text = text; }
        public void setLevel(string text) { Level.Text = text; }
        public void setMethods(string text)
        {
            string methodName = "setMethods()";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start data : {1}---", methodName, text));
            Methods.Text = text;
           
            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public void setMessage(string text)
        {
            string methodName = "setMessage()";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start data : {1}---", methodName, text));
            Message.Text = text;

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public void setClass(string text)
        {
            string methodName = "setClass()";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start data : {1}---", methodName, text));
            classes.Text = text;
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