
using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MyAttandance.Adapters;
using MyAttandance.Dialogs;
using MyAttandance.Entities;
using MyAttandance.Helper;
using static MyAttandance.Dialogs.FilterDialog;
using static MyAttandance.Helper.HttpClientHelper;
using static MyAttandance.ViewHolders.NotesVerificationHolder;

namespace MyAttandance
{
    public class NotesVerificationFragment : Fragment, IHttpClient, ifBoxIsChecked, IOnSubmitListener
    {
        private static string LOG_TAG = typeof(NotesVerificationFragment).Name;

        private NotesVerificationAdapter mAdapter;
        private List<Notes> noteList;
        private ListView mTableView;

        private Dictionary<int, int> checkedBox = new Dictionary<int, int>();

        private string fromFilterDate = null;
        private string toFilterDate = null;

        private List<string> userList = new List<string>();
        private List<string> typeList = new List<string>();
        private List<string> statusList = new List<string> { "Unapproved", "Approved", "Rejected" };

        private List<string> selectedUsers = new List<string>();
        private List<string> selectedTypes = new List<string>();
        private List<string> selectedStats = new List<string>();

        private List<int> finalSelectedNotes = new List<int>();

        private bool mainBoxChecked = false;

        private string thisDeviceMac = null;
        public static int lastStatus = 1;
        public static int lastCategory = 0;

        private CheckBox mainBox;
        private Context mContext;

        public Menu menu;

        private Dictionary<string, string> members = new Dictionary<string, string>();
        private Dictionary<string, string> types = new Dictionary<string, string>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_notesVerification, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            thisDeviceMac = WifiConnectivityHelper.getMacMarshmellow();

            mContext = Application.Context;
            mAdapter = new NotesVerificationAdapter(mContext, new List<Notes>(), this);
            mTableView = Activity.FindViewById<ListView>(Resource.Id.notes_rectable);
            mTableView.Adapter = mAdapter;

            mainBox = Activity.FindViewById<CheckBox>(Resource.Id.notes_checkall);

            populateType();
            userList = new List<string>(members.Keys);
            typeList = new List<string>(types.Keys);

            retrieveData(null, null, null, null, null);
        }


        public void AsyncResponse(string response)
        {
            noteList = HttpResponseHelper.retrieveNotesResponse(response);
            mAdapter.NotifyDataSetChanged();
        }

        public void ifChecked(int position, bool isChecked)
        {
            if (isChecked)
            {
                if (!checkedBox.ContainsKey(position))
                {
                    checkedBox.Add(position, noteList[position].id);
                }
            }
            else if (checkedBox.ContainsKey(position))
            {
                checkedBox.Remove(position);
            }

            this.mAdapter.updateCheckedBox(checkedBox.Keys.ToList());
        }

        //region Functions
        public void retrieveData(string[] macFilter, string fDateFilter, string tDateFilter, string[] category, string[] status)
        {
            DateTime fDates = DateTime.Now, tDates = DateTime.Now;

            if (fDateFilter != null && tDateFilter != null)
            {
                fDates = DateFormatingHelper.stringToDateWithFormat(fDateFilter, DateFormatingHelper.ShortDateOnlyFormat);
                tDates = DateFormatingHelper.stringToDateWithFormat(tDateFilter, DateFormatingHelper.ShortDateOnlyFormat);
            }

            DateTime today = DateTime.Now;
            today = today.AddMonths(-2);
            DateTime firstDateOfMonth = new DateTime(today.Year, today.Month, 1);

            string remoteSourceDateFormat = DateFormatingHelper.remoteSourceDateFormat;

            if (macFilter == null) macFilter = new string[] { thisDeviceMac };

            string fromDate = (fDateFilter == null) ?
                                    firstDateOfMonth.ToString(remoteSourceDateFormat) :
                                       fDates.ToString(remoteSourceDateFormat);

            string toDate = (tDateFilter == null) ?
                                    firstDateOfMonth.AddMonths(1).ToString(remoteSourceDateFormat) :
                                        tDates.ToString(remoteSourceDateFormat);

            category = (category == null) ? new string[] { "All" } : category;
            status = (status == null) ? new string[] { "Unapparoved" } : status;

            retrieveRequest(macFilter, fromDate, toDate, category, status);
        }

        private void retrieveRequest(string[] macFilter, string fDateFilter, string tDateFilter, string[] type, string[] status)
        {
            FetchDataAsync(HttpRequestHelper.notesRequestString(macFilter, fDateFilter, tDateFilter, type, status), this);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.search:
                    new FilterDialog(Activity, 0, this, fromFilterDate, toFilterDate, userList, typeList, null,
                     selectedUsers, selectedTypes, null, true, false, true,
                     (selectedTypes.Count == typeList.Count), (selectedUsers.Count == userList.Count)).Show();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }


        public void ifConfirm(int position, string from)
        {
            
        }

        public void onContentSubmit(string fromDate, string toDate, List<string> userMacs, List<string> notesTypes, List<string> notesStatuses)
        {
            this.fromFilterDate = fromDate;
            this.toFilterDate = toDate;
            this.selectedUsers = userMacs;
            this.selectedTypes = notesTypes;
            this.selectedStats = notesStatuses;

            retrieveData(selectedUsers.ToArray(), fromFilterDate, toFilterDate, selectedTypes.ToArray(), selectedStats.ToArray());
        }

        private void populateType()
        {
            types.Clear();
            types.Add("Sick", "1");
            types.Add("Allowed Late", "3");
            types.Add("Paid Leave", "4");
            types.Add("Early Leave", "5");
            types.Add("Overtime", "6");
        }
    }
}