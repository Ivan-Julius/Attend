
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using MyAttandance.Adapters;
using MyAttandance.Helper;
using MyAttandance.Entities;
using Android.Widget;

using System;
using static MyAttandance.Helper.HttpClientHelper;

namespace MyAttandance
{
    public class MonthlyReportFragment : Fragment, IHttpClient
    {
        private static string LOG_TAG = typeof(DailyReportFragment).Name;

        private string thisDeviceMac;
        private string fromFilterDate = null;
        private string toFilterDate = null;
        private string[] macList = null;
        private Dictionary<string, string> userList;
        private List<string> selectedUsers = new List<string>();

        private Context mContext;
        private bool allIsSelected = false;

        private MonthlyFragmentAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string methodName = "OnCreate";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            //SetHasOptionsMenu(true);
            RetainInstance = true;

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.fragment_monthlyReport, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            string methodName = "OnActivityCreated";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            Context mContext = Application.Context;
            thisDeviceMac = WifiConnectivityHelper.getMacMarshmellow();

            /*
            string user = ""; userList.TryGetValue(thisDeviceMac, out user);
            if (user.Length > 0) selectedUsers.Add(user);
            */

            mAdapter = new MonthlyFragmentAdapter(mContext, new List<MonthlyReport>());
            ListView mTableView = Activity.FindViewById<ListView>(Resource.Id.ltable);
            mTableView.Adapter = mAdapter;
            retrieveData(null, null, null);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public void retrieveData(string[] macFilter, string fDateFilter, string tDateFilter)
        {
            DateTime fDates = DateTime.Now, tDates = DateTime.Now;

            if (fDateFilter != null && tDateFilter != null)
            {
                fDates = DateFormatingHelper.stringToDateWithFormat(fDateFilter, DateFormatingHelper.ShortDateOnlyFormat);
                tDates = DateFormatingHelper.stringToDateWithFormat(tDateFilter, DateFormatingHelper.ShortDateOnlyFormat);
            }

            DateTime today = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(today.Year, today.Month, 1);

            string remoteSourceDateFormat = DateFormatingHelper.remoteSourceDateFormat;

            if (macFilter == null) macFilter = new string[] { thisDeviceMac };

            string fromDate = (fDateFilter == null) ?
                                    firstDateOfMonth.ToString(remoteSourceDateFormat) :
                                       fDates.ToString(remoteSourceDateFormat);

            string toDate = (tDateFilter == null) ?
                                    firstDateOfMonth.AddMonths(1).ToString(remoteSourceDateFormat) :
                                        tDates.ToString(remoteSourceDateFormat);

            retrieveRequest(macFilter, fromDate, toDate);
        }

        private void retrieveRequest(string[] macFilter, string fDateFilter, string tDateFilter)
        {
            FetchDataAsync(HttpRequestHelper.monthlyReportString(
                    WifiConnectivityHelper.getMacMarshmellow(), macFilter, fDateFilter, tDateFilter), this);
        }

        public void AsyncResponse(string response)
        {
            mAdapter.updateData(HttpResponseHelper.reportMonthlyResponse(response));
            mAdapter.NotifyDataSetChanged();

        }

        /*
        private void updateMacList(List<string> result)
        {

            string methodName = "updateMacList()";
            Log.Debug(LOG_TAG, string.Format("%s: --- start ---", methodName));

            selectedUsers = result;
            List<string> results = new List<string>();

            foreach (string name in result)
            {

                Log.Debug(LOG_TAG, string.Format("%s: --- %s ---", methodName, name));
                if (userList.containsValue(name))
                {
                    results.addAll(getKeyWithValue(name));
                }
            }

            macList = results.toArray(new String[results.size()]);


            Log.Debug(LOG_TAG, string.Format("%s: --- end ---", methodName));
        }*/

        public void setUserList(Dictionary<string, string> data)
        {
            this.userList = data;
        }

    }
}