
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Attend.Entities;
using Attend.Helper;
using Attend.HelperAndroid;
using DevExpress.Mobile.DataGrid;
using DevExpress.Mobile.DataGrid.Theme;
using Xamarin.Forms;
using static Attend.HelperAndroid.HttpClientHelper;
using static Attend.Pages.DateFilterOnlyPage;
using static Attend.Pages.FilterPage;

namespace Attend.Pages
{
    public class DailyReportsPage : ContentPage, IHttpClient, IOnSubmit, IOnSubmitDateFilter
    {
        private static string LOG_TAG = typeof(DailyReportsPage).ToString();
        private ObservableCollection<DailyReport> dailyReportList = new ObservableCollection<DailyReport>();
        private GridControl exDevGrid = new GridControl();

        private static List<string> selectedMembers = new List<string>();
        private static List<string> selectedType = new List<string>();
        private static List<string> selectedStatus = new List<string>();

        private List<string> Statuses = new List<string> { "Late", "Not Full", "Overtime" };

        private Dictionary<string, string> members = new Dictionary<string, string>();
        private Dictionary<string, string> Types = new Dictionary<string, string>();

        private string fromDate = "";
        private string toDate = "";

        public DailyReportsPage() {

            obtainMembers();
            populateType();

            exDevGrid = new GridControl
            {
                AllowEditRows = false,
                AllowDeleteRows = false,
                AllowHorizontalScrollingVirtualization = true,
                AllowResizeColumns = false,
                AllowSort = false,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ItemsSource = dailyReportList, 
                RowHeight = 80       
            };

            ColumnSetFieldNames();

            ThemeManager.ThemeName = Themes.Light;
            Content = exDevGrid;

            ToolbarItem filter = new ToolbarItem { Text = "Filter" };

            filter.Clicked += delegate
            {
                OnFilterClicked();
            };

            ToolbarItems.Add(filter);

            retrieveData(null, null, null, null, null);
        }

        private void retrieveData(string[] macFilter, string fDateFilter, string tDateFilter, string[] typeFilter, string[] statusFilter)
        {
            DateTime fDates = DateTime.Now, tDates = DateTime.Now;

            DateTime today = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(today.Year, today.Month, 1);

            string remoteSourceDateFormat = DateFormatingHelper.remoteSourceDateFormat;

            if (macFilter == null) macFilter = new string[] { App.thisDeviceMac };

            string fromDate = (fDateFilter == null) ?
                                    firstDateOfMonth.ToString(remoteSourceDateFormat) : fDateFilter;

            string toDate = (tDateFilter == null) ?
                                    firstDateOfMonth.AddMonths(1).ToString(remoteSourceDateFormat) : tDateFilter;

            if (typeFilter == null) typeFilter = new string[] { "1", "3", "4", "5", "6", "8" };

            retrieveRequest(macFilter, fromDate, toDate, typeFilter, statusFilter);
        }

        private void retrieveRequest(string[] macFilter, string fDateFilter, string tDateFilter, string[] typeList, string[] statusFilter)
        {
            Debug.WriteLine("from : " + fDateFilter);
            Debug.WriteLine("to : " + tDateFilter);

            FetchDataAsync(HttpRequestHelper.dailyReportString(
              App.thisDeviceMac, macFilter, fDateFilter, tDateFilter, typeList, statusFilter), this, "Daily");
        }

        public void AsyncResponse(string response, string from)
        {

            if (from != "Member")
            {
                dailyReportList = new ObservableCollection<DailyReport>(HttpResponseHelper.reportDailyResponse(response));
                exDevGrid.ItemsSource = dailyReportList;
                ColumnSetFieldNames();
            }else
            {
                members = HttpResponseHelper.responseMemberList(response);
                App.members = members;
            }
        }

        private async void OnFilterClicked()
        {
            if (App.members.Count > 0)
            {
                await Navigation.PushModalAsync(new FilterPage(fromDate, toDate, new List<string>(members.Keys), new List<string>(Types.Keys), Statuses, selectedMembers, selectedType, selectedStatus, true, true, this), false);

            } else
            {
                await Navigation.PushModalAsync(new DateFilterOnlyPage(fromDate, toDate, this), false);
            }
        }

        private void ColumnSetFieldNames()
        {
            try
            {
                exDevGrid.Columns["is_full"].Caption = "Full";
                exDevGrid.Columns["is_late"].IsVisible = false;

                exDevGrid.Columns["login_elapse"].Caption = "Elapses";

                exDevGrid.Columns["id"].Caption = "No";
                exDevGrid.Columns["id"].Width = 10;

                exDevGrid.Columns["user_name"].Caption = "User";
                exDevGrid.Columns["overtime_elapse"].Caption = "OverElapses";
                exDevGrid.Columns["date"].Caption = "Date";

                exDevGrid.Columns["date"].MinWidth = 50;

                exDevGrid.Columns["first_login"].Caption = "First";
                exDevGrid.Columns["last_login"].Caption = "Last";
                exDevGrid.Columns["logout_time"].Caption = "Day End";
                exDevGrid.Columns["start_overtime"].Caption = "Overtime";
                exDevGrid.Columns["login_type"].Caption = "Status";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR : " + ex.Message);
            }

            exDevGrid.FormatConditions.Add(new FormatCondition
            {
                FieldName = "first_login",
                ValueRule = DevExpress.Mobile.Core.ConditionalFormatting.ConditionRule.LessOrEqual,
                Value1 = "11:00:00",
                PredefinedFormatName = "LightGreenFill"
            });

            exDevGrid.FormatConditions.Add(new FormatCondition
            {
                FieldName = "first_login",
                ValueRule = DevExpress.Mobile.Core.ConditionalFormatting.ConditionRule.Greater,
                Value1 = "11:00:00",
                PredefinedFormatName = "LightRedFill"
            });
        }

        private void populateType()
        {
            Types.Clear();
            Types.Add("Sick", "1");
            Types.Add("Allowed Late", "3");
            Types.Add("Paid Leave", "4");
            Types.Add("Early Leave", "5");
            Types.Add("Normal", "6");
            Types.Add("Absent", "8");
        }

        public void OnSubmit(string fromDate, string toDate, List<string> currentSelectedMembers, List<string> currentSelectedTypes, List<string> currentSelectedStatuses)
        {
            selectedMembers = currentSelectedMembers;
            selectedType = currentSelectedTypes;
            selectedStatus = currentSelectedStatuses;
            this.fromDate = fromDate;
            this.toDate = toDate;

            string[] toSendMembers = members.Where(p => selectedMembers.Contains(p.Key)).Select(x => x.Value).ToArray();
            string[] toSendTypes = Types.Where(p => selectedType.Contains(p.Key)).Select(x => x.Value).ToArray();

            foreach(string i  in toSendMembers)
            {
                Debug.WriteLine("Members : " + i);
            }

            retrieveData(toSendMembers,fromDate, toDate, toSendTypes, selectedStatus.ToArray());
            closeDialog();
        }

        public void OnCancel()
        {
            closeDialog();
        }

        private void closeDialog()
        {
            Navigation.PopModalAsync(false);
        }

        public void OnSubmit(string fromDate, string toDate)
        {
            this.fromDate = fromDate;
            this.toDate = toDate;

            retrieveData(null, fromDate, toDate, null, null);
            closeDialog();
        }

        private void obtainMembers()
        {
            if (App.members.Count > 0)
                members = App.members;
            else
                App.aqcuiredMembers(this);
        }
    }
}
