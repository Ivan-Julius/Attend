
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
using MyAttandance.SqlSource;
using MyAttandance.Dialogs;
using static MyAttandance.Dialogs.FilterDialog;
using static MyAttandance.Helper.HttpClientHelper;

namespace MyAttandance
{
    public class DailyReportFragment : Fragment, IHttpClient, IOnSubmitListener
    {
        private static string LOG_TAG = typeof(DailyReportFragment).Name;

        private string thisDeviceMac = null;
        LogDataSource<GeneralLog> genlog = null;

        private string fromFilterDate = null;
        private string toFilterDate = null;

        private List<string> userList = new List<string> ();
        private List<string> typeList = new List<string> ();
        private List<string> statusList = new List<string> { "Overtime", "Late", "Not Full" };

        //private Dictionary<string, string> userList;
        private List<string> selectedUsers = new List<string>();
        private List<string> selectedTypes = new List<string>();
        private List<string> selectedStats = new List<string>();

        private DailyFragmentAdapter mAdapter;
        private Dictionary<string, string> members = new Dictionary<string, string>();
        private Dictionary<string, string> types = new Dictionary<string, string>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            genlog = new LogDataSource<GeneralLog>();

            SetHasOptionsMenu(true);
            RetainInstance = true; 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_dailyReport, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            string methodName = "OnActivityCreated";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));
            genlog.Debug(Application.Context, new List<string> { LOG_TAG, methodName, string.Format("----- start -----") });

            Context mContext = Application.Context;
            thisDeviceMac = WifiConnectivityHelper.getMacMarshmellow();

            mAdapter = new DailyFragmentAdapter(mContext, new List<DailyReport>());
            ListView mTableView = Activity.FindViewById<ListView>(Resource.Id.tableList);
            mTableView.Adapter = mAdapter;

            populateType();

            userList = new List<string>(members.Keys);
            typeList = new List<string>(types.Keys);

            retrieveData(null, null, null, null, null);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.search:
                    new FilterDialog(Activity, 0, this, fromFilterDate, toFilterDate, userList, typeList, statusList,
                     selectedUsers, selectedTypes, selectedStats, true, true, true, 
                     (selectedTypes.Count == typeList.Count), (selectedUsers.Count == userList.Count)).Show();
                break;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnSaveInstanceState(Bundle bundle)
        {
            base.OnSaveInstanceState(bundle);
            //members = MainActivity.members;
        }

        private void retrieveData(string[] macFilter, string fDateFilter, string tDateFilter, string[] typeFilter, string[] statusFilter)
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

            if (typeFilter == null) typeFilter = new string[]{ "1", "3", "4", "5", "6", "8" };

            retrieveRequest(macFilter, fromDate, toDate, typeFilter, statusFilter);
        }

        private void retrieveRequest(string[] macFilter, string fDateFilter, string tDateFilter, string[] typeList, string[] statusFilter)
        {
            FetchDataAsync(HttpRequestHelper.dailyReportString(
              WifiConnectivityHelper.getMacMarshmellow(), macFilter, fDateFilter, tDateFilter, typeList, statusFilter), this);
        }

        public void AsyncResponse(string response)
        {
            mAdapter.updateData(HttpResponseHelper.reportDailyResponse(response));
            mAdapter.NotifyDataSetChanged();
        }

        public void onContentSubmit(string fromDate, string toDate, List<string> userMacs, List<string> attandanceType, List<string> attandanceStatus)
        {
            this.fromFilterDate = fromDate;
            this.toFilterDate = toDate;
            this.selectedUsers = userMacs;
            this.selectedTypes = attandanceType;
            this.selectedStats = attandanceStatus;

            retrieveRequest(selectedGetStringValue(selectedUsers, members).ToArray(),
                fromFilterDate, toFilterDate, selectedGetStringValue(selectedTypes, types).ToArray(), selectedStats.ToArray());
        }

        public List<string> selectedGetStringValue(List<string> selected, Dictionary<string, string> fromPlace)
        {
            List<string> values = new List<string>();
            foreach(string i in selected)
            {
                string toAdd = "";
                fromPlace.TryGetValue(i, out toAdd);

                if (toAdd != null) values.Add(toAdd);
            }

            return values;
        }

        private void populateType()
        {
            types.Clear();
            types.Add("Sick", "1");
            types.Add("Allowed Late", "3");
            types.Add("Paid Leave", "4");
            types.Add("Early Leave", "5");
            types.Add("Normal", "6");
            types.Add("Absent", "8");
        }

        public void updateUserList(Dictionary<string,string> users)
        {
            members = users;
        }

    }
}