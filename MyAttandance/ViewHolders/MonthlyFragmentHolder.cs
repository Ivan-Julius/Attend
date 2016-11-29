
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MyAttandance.ViewHolders
{
    class MonthlyFragmentHolder : Object
    {
        private static string LOG_TAG = typeof(MonthlyFragmentHolder).Name;

        private TextView report_owner;
        private TextView report_year;
        private TextView report_month;
        private TextView count_late;
        private TextView count_not_full;
        private TextView count_full;
        private TextView count_sick;
        private TextView count_leave;
        private TextView count_AllowedLate;
        private TextView count_earlyLeave;
        private TextView overtime_elapse_time;
        private TextView count_not_login;
        private TextView count_login;
        private TextView total_attendance;

        public void setReport_owner(string text)
        {
            report_owner.Text = text;
        }

        public void setReport_year(string text) { report_year.Text = text; }
        public void setReport_month(string text) { report_month.Text = text; }
        public void setCount_late(string text) { count_late.Text = text; }
        public void setCount_not_full(string text) { count_not_full.Text = text; }
        public void setCount_full(string text) { count_full.Text = text; }
        public void setCount_sick(string text) { count_sick.Text = text; }
        public void setCount_leave(string text) { count_leave.Text = text; }
        public void setCount_AllowedLate(string text) { count_AllowedLate.Text = text; }
        public void setCount_earlyLeave(string text) { count_earlyLeave.Text = text; }
        public void setOvertime_elapse_time(string text) { overtime_elapse_time.Text = text; }
        public void setCount_not_login(string text) { count_not_login.Text = text; }
        public void setCount_login(string text) { count_login.Text = text; }
        public void setTotal_attendance(string text) { total_attendance.Text = text; }

        public void setView(View v)
        {

            report_owner = (TextView)v.FindViewById(Resource.Id.reports_report_owner);
            report_year = (TextView)v.FindViewById(Resource.Id.reports_report_year);
            report_month = (TextView)v.FindViewById(Resource.Id.reports_report_month);
            count_late = (TextView)v.FindViewById(Resource.Id.reports_count_late);
            count_not_full = (TextView)v.FindViewById(Resource.Id.reports_count_not_full);
            count_full = (TextView)v.FindViewById(Resource.Id.reports_count_full);
            count_sick = (TextView)v.FindViewById(Resource.Id.reports_count_sick);
            count_leave = (TextView)v.FindViewById(Resource.Id.reports_count_leave);
            count_earlyLeave = (TextView)v.FindViewById(Resource.Id.reports_count_earlyLeave);
            overtime_elapse_time = (TextView)v.FindViewById(Resource.Id.reports_overtime_elapse_time);
            count_not_login = (TextView)v.FindViewById(Resource.Id.reports_count_not_login);
            count_AllowedLate = (TextView)v.FindViewById(Resource.Id.reports_count_AllowedLate);
            count_login = (TextView)v.FindViewById(Resource.Id.reports_count_login);
            total_attendance = (TextView)v.FindViewById(Resource.Id.reports_total_attendance);
        }

        //set all custom listeners inside
        public void setListeners(int viewPosition)
        {

            string methodName = "setListeners";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start : viewPosition{1}---", methodName, viewPosition.ToString()));

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }
    }
}