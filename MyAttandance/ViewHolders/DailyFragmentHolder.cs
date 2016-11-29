
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MyAttandance.Helper;

namespace MyAttandance.ViewHolders
{
    class DailyFragmentHolder : Object
    { 
        private static string LOG_TAG = typeof(DailyFragmentHolder).Name;

        //Field Declaration
        public TextView StrId;
        public TextView date;
        public TextView First_login;
        public TextView Last_login;
        public TextView Logout_time;
        public TextView Start_overtime;
        public TextView StrLoginElapse;
        public TextView StrOvertimeElapse;
        public TextView StrIsLate;
        public TextView StrIsFull;
        public TextView Login_type;
        public TextView User_name;

        //Input method declaration
        public void setStrId(string text) { StrId.Text = text; }
        public void setDate(string text) { date.Text = DateFormatingHelper.StringToStringWithFormat(text, DateFormatingHelper.WithDayNameFormat); }
        public void setFirst_login(string text) { First_login.Text = DateFormatingHelper.StringToStringWithFormat(text, DateFormatingHelper.OnlyTimeTwentyFourHoursFormat); }
        public void setLast_login(string text) { Last_login.Text = DateFormatingHelper.StringToStringWithFormat(text, DateFormatingHelper.OnlyTimeTwentyFourHoursFormat); }
        public void setLogout_time(string text) { Logout_time.Text = DateFormatingHelper.StringToStringWithFormat(text, DateFormatingHelper.OnlyTimeTwentyFourHoursFormat); }
        public void setStart_overtime(string text) { Start_overtime.Text = DateFormatingHelper.StringToStringWithFormat(text, DateFormatingHelper.OnlyTimeTwentyFourHoursFormat); }
        public void setStrLoginElapse(string fromtext, string totext) { StrLoginElapse.Text = DateFormatingHelper.stringToTimeElapses(fromtext, totext); }
        public void setStrIsLate(string text) { StrIsLate.Text = text; }
        public void setStrIsFull(string text) { StrIsFull.Text = text; }
        public void setStrOvertime(string fromtext, string totext) { StrOvertimeElapse.Text = DateFormatingHelper.stringToTimeElapses(fromtext, totext); }
        public void setLogin_type(string text) { Login_type.Text = text; }
        public void setUser_name(string text) { User_name.Text = text; }
        public void setFirstLoginBG(string text) { First_login.SetBackgroundColor((text.ToLower().Equals("late") ? Color.Red : Color.Green)); }
        public void setLastLoginBG(string text) { Last_login.SetBackgroundColor((text.ToLower().Equals("full") ? Color.Green : Color.Red)); }


        public void SetView(View v)
        {
            StrId = (TextView)v.FindViewById(Resource.Id.reports_id);
            date = (TextView)v.FindViewById(Resource.Id.reports_date);
            First_login = (TextView)v.FindViewById(Resource.Id.reports_first_login);
            Last_login = (TextView)v.FindViewById(Resource.Id.reports_last_login);
            Logout_time = (TextView)v.FindViewById(Resource.Id.reports_logout_time);
            Start_overtime = (TextView)v.FindViewById(Resource.Id.reports_start_overtime);
            StrLoginElapse = (TextView)v.FindViewById(Resource.Id.reports_login_elapse);
            StrOvertimeElapse = (TextView)v.FindViewById(Resource.Id.reports_overtimeElapses);
            StrIsLate = (TextView)v.FindViewById(Resource.Id.reports_is_late);
            StrIsFull = (TextView)v.FindViewById(Resource.Id.reports_is_full);
            Login_type = (TextView)v.FindViewById(Resource.Id.reports_login_type);
            User_name = (TextView)v.FindViewById(Resource.Id.reports_user_name);
        }

        //set all custom listeners inside
        public void SetListeners(int viewPosition)
        {

            string methodName = "setListeners";

            Log.Debug(LOG_TAG, string.Format("{0}: --- start : viewPosition{1}---", methodName, viewPosition.ToString()));

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

    }
}