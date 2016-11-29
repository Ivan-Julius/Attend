
using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using MyAttandance.Adapters;
using MyAttandance.Helper;

namespace MyAttandance.Dialogs
{
    public class FilterDialog : AlertDialog, CheckboxDialogAdapter.checkboxSelected, IDialogInterfaceOnShowListener, DatePickerDialog.IOnDateSetListener
    {
        private static string LOG_TAG = typeof(FilterDialog).Name;

        private static string from = "";
        private static string to = "";
        private static List<string> macs = new List<string>();
        private static List<string> types = new List<string>();
        private static List<string> statuses = new List<string>();

        private IOnSubmitListener listener = null;
        private Context mContext = null;
        private LayoutInflater inflater = null;

        private TextView fromDateView = null;
        private TextView toDateView = null;

        private static CheckboxDialogAdapter membersAdapter;
        private static CheckboxDialogAdapter typesAdapter;
        private static CheckboxDialogAdapter statusAdapter;

        private ListView userList = null;
        private ListView typeList = null;
        private ListView statusList = null;

        private List<string> selectedMacs = new List<string>();
        private List<string> selectedType = new List<string>();
        private List<string> selectedStatuses = new List<string>();

        private CheckBox typeHeaderChecker = null;
        private CheckBox userHeaderChecker = null;

        private Calendar calendar = Calendar.Instance;

        private bool isFrom = false;

        public interface IOnSubmitListener
        {
            void onContentSubmit(string fromDate, string toDate, List<string> userMacs, List<string> attandanceType, List<string> attandanceStatus);
        }

        static int resolveDialogTheme(Context context, int resId)
        {
            if (resId == 0)
            {
                TypedValue outValue = new TypedValue();
                context.Theme.ResolveAttribute(Resource.Attribute.popupTheme, outValue, true);
                return outValue.ResourceId;
            }
            else
            {
                return resId;
            }
        }

        public FilterDialog(Context context, int theme, IOnSubmitListener listener, string fromDate, string toDate, List<string> userMacs, List<string> attandanceType, List<string> attandanceStatus,
            List<string> selectedUsers, List<string> selectedTypes, List<string> selectedStats, bool withMembers, bool withStatus, bool withType, bool typeAll, bool userAll) :
            base(context, resolveDialogTheme(context, theme))
        {
            this.listener = listener;
            this.SetOnShowListener(this);
            this.inflater = LayoutInflater.From(context);

            from = (fromDate == null)? "" : fromDate;
            to = (toDate == null)? "" : toDate;
            macs = userMacs;
            types = attandanceType;
            statuses = attandanceStatus;
            selectedMacs = selectedUsers;
            selectedType = selectedTypes;
            selectedType = selectedStats;

            mContext = Application.Context;
            LayoutInflater inflater = LayoutInflater.From(mContext);

            View view = inflater.Inflate(Resource.Layout.dialog_filters, null);

            fromDateView = (TextView)view.FindViewById(Resource.Id.fromDateReceiver);
            toDateView = (TextView)view.FindViewById(Resource.Id.toDateReceiver);

            fromDateView.Click += delegate
            {
                isFrom = true;              
                new DatePickerDialog(context, this, calendar.Get(CalendarField.Year), calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth)).Show();
            };

            toDateView.Click += delegate
            {
                isFrom = false;      
                new DatePickerDialog(context, this, calendar.Get(CalendarField.Year), calendar.Get(CalendarField.Month), calendar.Get(CalendarField.DayOfMonth)).Show();
            };

            userList = view.FindViewById<ListView>(Resource.Id.options_users);
            membersAdapter = new CheckboxDialogAdapter(mContext, "member", userMacs, this);
            userList.Adapter = membersAdapter;
            userList.AddHeaderView(addListCheckBox(membersAdapter, "member", userAll));
            membersAdapter.updateSelected(selectedUsers);

            typeList = view.FindViewById<ListView>(Resource.Id.options_types);
            typesAdapter = new CheckboxDialogAdapter(mContext, "type", attandanceType, this);
            typeList.Adapter = typesAdapter;
            typeList.AddHeaderView(addListCheckBox(typesAdapter, "type", typeAll));
            typesAdapter.updateSelected(selectedTypes);

            statusList = view.FindViewById<ListView>(Resource.Id.options_status);
            statusAdapter = new CheckboxDialogAdapter(mContext, "status", attandanceStatus, this);
            statusList.Adapter = statusAdapter;
            statusAdapter.updateSelected(selectedStats);
            
            fromDateView.Text = formater(true, (from.Length > 0) ? DateFormatingHelper.StringToStringWithFormat(from, DateFormatingHelper.ShortDateWithTimeFormat) : DateTime.Now.ToString(DateFormatingHelper.ShortDateWithTimeFormat));
            toDateView.Text = formater(true, (to.Length > 0) ? DateFormatingHelper.StringToStringWithFormat(to, DateFormatingHelper.ShortDateWithTimeFormat) : DateTime.Now.ToString(DateFormatingHelper.ShortDateWithTimeFormat));

            SetView(view);

            SetButton((int)DialogButtonType.Positive, new Java.Lang.String("Confirm"), delegate {
                this.DialogConfirm();
            });

            SetButton((int)DialogButtonType.Negative, new Java.Lang.String("Cancel"), delegate {
                Cancel();
            });

            typeList.Visibility = (withType) ? ViewStates.Visible : ViewStates.Gone;
            userList.Visibility = (withMembers) ? ViewStates.Visible : ViewStates.Gone;
            statusList.Visibility = (withStatus) ? ViewStates.Visible : ViewStates.Gone;
        }

        private View addListCheckBox(CheckboxDialogAdapter adapters, string from, bool isChecked)
        {
            View header = inflater.Inflate(Resource.Layout.dialog_option_list, null);
            CheckBox Checker = (CheckBox)header.FindViewById(Resource.Id.options_box);
            Checker.Text = "All";
            Checker.Checked = isChecked;

            switch (from)
            {
                case "type":

                    Checker.Click += delegate
                    {
                        typeAllSelected();
                    };

                    typeHeaderChecker = Checker;

                break;

                case "member":

                    Checker.Click += delegate
                    {
                         memberAllSelected();
                    };

                    userHeaderChecker = Checker;

                break;
            }

            return header;
        }

        public void typeAllSelected()
        {
            foreach (string i in types)
            {
                onCheckBoxSelected(i, typeHeaderChecker.Checked, "type");
            }      
        }

        public void memberAllSelected()
        {
            foreach (string i in macs)
            {
                onCheckBoxSelected(i, userHeaderChecker.Checked, "member");
            }
        }

        public void onCheckBoxSelected(string content, bool check, string type)
        {      
            switch (type)
            {
                case "type":

                    if (selectedType.Contains(content)) selectedType.Remove(content);
                    if (check)
                    {
                        selectedType.Add(content);
                        if (selectedType.Count == types.Count) typeHeaderChecker.Checked = true;
                    }
                    else
                        typeHeaderChecker.Checked = false;

                    typesAdapter.updateSelected(selectedType);
                    typesAdapter.NotifyDataSetChanged();

                    break;

                case "member":

                    if (selectedMacs.Contains(content)) selectedMacs.Remove(content);
                    if (check)
                    {
                        selectedMacs.Add(content);
                        if (selectedMacs.Count == macs.Count) userHeaderChecker.Checked = true;
                    }
                    else userHeaderChecker.Checked = false;

                    membersAdapter.updateSelected(selectedMacs);
                    membersAdapter.NotifyDataSetChanged();

                break;

                case "status":

                if (selectedStatuses.Contains(content)) selectedStatuses.Remove(content);
                if (check) selectedStatuses.Add(content);

                statusAdapter.updateSelected(selectedStatuses);
                statusAdapter.NotifyDataSetChanged();
                break;
            }
        }

        public void OnShow(IDialogInterface dialog)
        {
            AlertDialog alertDialog = (AlertDialog)dialog;
            WindowManagerLayoutParams layoutParams = new WindowManagerLayoutParams();
            layoutParams.CopyFrom(alertDialog.Window.Attributes);

            layoutParams.Width = WindowManagerLayoutParams.MatchParent; // 80% of screen
            layoutParams.Gravity = GravityFlags.Left;

            alertDialog.Window.Attributes = layoutParams;
        }

        public void DialogConfirm()
        {
            Log.Debug(LOG_TAG, string.Format("{0} from : {1}", "TestingDataEjections",from.ToString()));
            Log.Debug(LOG_TAG, string.Format("{0} from : {1}", "TestingDataEjections", to.ToString()));
            Log.Debug(LOG_TAG, string.Format("{0} from : {1}", "TestingDataEjections", selectedMacs.Count.ToString()));
            Log.Debug(LOG_TAG, string.Format("{0} from : {1}", "TestingDataEjections", selectedType.Count.ToString()));
            Log.Debug(LOG_TAG, string.Format("{0} from : {1}", "TestingDataEjections", selectedStatuses.Count.ToString()));

            if (listener != null) listener.onContentSubmit(from, to, selectedMacs, selectedType, selectedStatuses);
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            calendar.Set(year, monthOfYear, dayOfMonth);
            SimpleDateFormat dateFormatter = new SimpleDateFormat(DateFormatingHelper.ShortDateWithTimeFormat, Locale.Us);

            sendBackString((isFrom) ? fromDateView : toDateView,  dateFormatter.Format(calendar.Time), isFrom);
        }

        private void sendBackString(TextView view, string result, bool isFrom)
        {
            if(isFrom)from = formater(false, result);
            else to = formater(false, result);

            view.Text = formater(true, result);
        }

        private string formater(bool isForDisplay, string Data)
        {
            string result = "";

            System.DateTime date = DateFormatingHelper.stringToDateWithFormat(Data, DateFormatingHelper.ShortDateWithTimeFormat);
            result = (isForDisplay) ? date.ToString(DateFormatingHelper.LongDateOnlyFormat) : date.ToString(DateFormatingHelper.YearInFrontFormat);

            return result;
        }


    }
}