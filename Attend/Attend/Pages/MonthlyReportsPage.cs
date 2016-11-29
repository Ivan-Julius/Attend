
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
    public class MonthlyReportsPage : ContentPage, IHttpClient, IOnSubmit, IOnSubmitDateFilter
    {
        private static string LOG_TAG = typeof(MonthlyReportsPage).ToString();
        private ObservableCollection<MonthlyReport> monthlyReportList = new ObservableCollection<MonthlyReport>();
        private GridControl exDevGrid = new GridControl();

        private List<string> selectedMembers = new List<string>();
        private Dictionary<string, string> members = new Dictionary<string, string>();

        private string fromDate = "";
        private string toDate = "";

        public MonthlyReportsPage()
        {

            obtainMembers();

            exDevGrid = new GridControl
            {
                AllowEditRows = false,
                AllowDeleteRows = false,
                AllowHorizontalScrollingVirtualization = true,
                AllowResizeColumns = false,
                AllowSort = false,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ItemsSource = monthlyReportList,
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

            retrieveData(null, null, null);
        }

        private void ColumnSetFieldNames()
        {
            try
            {
                exDevGrid.Columns["report_year"].Caption = "Year";
                exDevGrid.Columns["report_month"].Caption = "Month";
                exDevGrid.Columns["name"].Caption = "User";
                exDevGrid.Columns["count_late"].Caption = "Late";
                exDevGrid.Columns["count_not_full"].IsVisible = false;
                exDevGrid.Columns["count_full"].Caption = "Full";
                exDevGrid.Columns["count_sick"].Caption = "Sick";
                exDevGrid.Columns["count_leave"].Caption = "Leave";
                exDevGrid.Columns["count_AllowedLate"].Caption = "A.Late";
                exDevGrid.Columns["count_earlyLeave"].Caption = "E.Leave";
                exDevGrid.Columns["count_not_login"].Caption = "Absent";
                exDevGrid.Columns["count_login"].IsVisible = false;
                exDevGrid.Columns["overtime_elapse_time"].Caption = "O.Elapse";
                exDevGrid.Columns["total_attendance"].Caption = "Total";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR : " + ex.Message);
            }
        } 

        private void retrieveData(string[] macFilter, string fDateFilter, string tDateFilter)
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

            retrieveRequest(macFilter, fromDate, toDate);
        }


        private void retrieveRequest(string[] macFilter, string fDateFilter, string tDateFilter)
        {
            Debug.WriteLine("from : " + fDateFilter);
            Debug.WriteLine("to : " + tDateFilter);

            monthlyReportList.Clear();

            FetchDataAsync(HttpRequestHelper.monthlyReportString(
              App.thisDeviceMac, macFilter, fDateFilter, tDateFilter), this, "Monthly");
        }

        public void AsyncResponse(string response, string from)
        {
            if (from != "Member")
            {
                monthlyReportList = new ObservableCollection<MonthlyReport>(HttpResponseHelper.reportMonthlyResponse(response));
                exDevGrid.ItemsSource = monthlyReportList;
                ColumnSetFieldNames();
            }
            else{

                members = HttpResponseHelper.responseMemberList(response);
                App.members = members;
            }
        }

        public void OnSubmit(string fromDate, string toDate, List<string> currentSelectedMembers, List<string> currentSelectedTypes, List<string> currentSelectedStatuses)
        {
            this.selectedMembers = currentSelectedMembers;
            this.fromDate = fromDate;
            this.toDate = toDate;

            string[] toSendMembers = members.Where(p => selectedMembers.Contains(p.Key)).Select(x => x.Value).ToArray();

            retrieveData(toSendMembers, fromDate, toDate);

            closeDialog();
        }

        public void OnCancel()
        {
            closeDialog();
        }

        private async void OnFilterClicked()
        {
            if (App.members.Count > 0)
            {
                await Navigation.PushModalAsync(new FilterPage(fromDate, toDate, new List<string>(members.Keys), new List<string>(), new List<string>(), selectedMembers, new List<string>(), new List<string>(), false, false, this), false);

            } else
            {
                await Navigation.PushModalAsync(new DateFilterOnlyPage(fromDate, toDate, this), false);
            }

        }

        private void closeDialog()
        {
            Navigation.PopModalAsync(false);
        }

        public void OnSubmit(string fromDate, string toDate)
        {
            this.fromDate = fromDate;
            this.toDate = toDate;

            retrieveData(null, fromDate, toDate);

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
