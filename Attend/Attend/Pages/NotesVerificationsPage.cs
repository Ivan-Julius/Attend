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
using static Attend.Pages.VerificationApprovalPage;
using static Attend.Pages.VerificationFilterPage;

namespace Attend.Pages
{
    public class NotesVerificationsPage : ContentPage, IHttpClient, IOnSubmitVFilter, IOnSubmitVApproval
    {
        private static string LOG_TAG = typeof(NotesVerificationsPage).ToString();
        private ObservableCollection<NotesVerification> notesVerificationList = new ObservableCollection<NotesVerification>();
        private GridControl exDevGrid = new GridControl();

        private List<string> selectedMembers = new List<string>();
        private string selectedType = "All";
        private string selectedStatus = "All";

        private string lastSelectedApproval = "Approved";

        private Dictionary<string, string> members = new Dictionary<string, string>();
        private Dictionary<string, string> Types = new Dictionary<string, string>();

        private List<string> status = new List<string>();
        private List<string> selectedRows = new List<string>();

        private bool isAllSelected = false; 

        private string fromDate = "";
        private string toDate = "";

        ToolbarItem selectAll = null;

        public NotesVerificationsPage()
        {
            obtainMembers();
            populateStatus();
            populateType();

            exDevGrid = new GridControl
            {
                AllowEditRows = false,
                AllowDeleteRows = false,
                AllowHorizontalScrollingVirtualization = true,
                AllowResizeColumns = false,
                AllowSort = false,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ItemsSource = notesVerificationList,
                RowHeight = 80,
            };

            exDevGrid.RowTap += ExDevGrid_RowTap;

            exDevGrid.SelectionChanged += delegate
            {

            };

            ColumnSetFieldNames();

            ThemeManager.ThemeName = Themes.Light;
            Content = exDevGrid;

            ToolbarItem filter = new ToolbarItem { Text = "Filter" };

            filter.Clicked += delegate
            {
                OnFilterClicked();
            };

            selectAll = new ToolbarItem { Text = "All" };
            selectAll.Clicked += SelectAll_Clicked;


            ToolbarItem approval = new ToolbarItem { Text = "Approval" };

            approval.Clicked += Approval_Clicked;

            ToolbarItems.Add(filter);
            ToolbarItems.Add(selectAll);
            ToolbarItems.Add(approval);

            retrieveData(null, null, null, null, null);
        }

        private async void Approval_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new VerificationApprovalPage(lastSelectedApproval, this), false);
        }

        private void SelectAll_Clicked(object sender, EventArgs e)
        {
            bool selection = !isAllSelected;

            for (int i = 0; i < notesVerificationList.Count(); i++)
            {
                string selected = exDevGrid.GetRow(i).GetFieldValue("id").ToString();
                if (selectedRows.Contains(selected)) selectedRows.Remove(selected);

                notesVerificationList[i].isSelected = (selection);
                exDevGrid.SetCellValue(i, "isSelected", (selection));
                if (selection) selectedRows.Add(selected);
            }

            isAllSelected = selection;

            ((ToolbarItem)sender).Text = (isAllSelected) ? "Not All" : "All";  
        }

        private void ExDevGrid_RowTap(object sender, RowTapEventArgs e)
        {
            string selected = exDevGrid.GetRow(e.RowHandle).GetFieldValue("id").ToString();

            if (selectedRows.Contains(selected)) selectedRows.Remove(selected);

            bool isSelected = (notesVerificationList[e.RowHandle].isSelected) ? true : false;
            notesVerificationList[e.RowHandle].isSelected = (!isSelected);
            exDevGrid.SetCellValue(e.RowHandle, "isSelected", (!isSelected));

            if (!isSelected) selectedRows.Add(selected);

            isAllSelected = (selectedRows.Count == notesVerificationList.Count)? true : false;

            ((ToolbarItem)selectAll).Text = (isAllSelected) ? "Not All" : "All";
        }

        private void ColumnSetFieldNames()
        {
            try
            {
                exDevGrid.Columns["id"].IsVisible = false;
                exDevGrid.Columns["fromDate"].Caption = "From";
                exDevGrid.Columns["toDate"].Caption = "To";
                exDevGrid.Columns["isSelected"].Caption = "Select";
                exDevGrid.Columns["noteType"].Caption = "Type";
                exDevGrid.Columns["status"].Caption = "Status";
                exDevGrid.Columns["reason"].Caption = "Reason";
                exDevGrid.Columns["createDate"].Caption = "Created";
                exDevGrid.Columns["owner"].Caption = "User";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR : " + ex.Message);
            }
        }

        private void retrieveData(string[] macFilter, string fDateFilter, string tDateFilter, string typeFilter, string statusFilter)
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

            typeFilter = (string.IsNullOrEmpty(typeFilter)) ? "All" : typeFilter;
            statusFilter = (string.IsNullOrEmpty(statusFilter)) ? "All" : statusFilter;

            retrieveRequest(macFilter, fromDate, toDate, typeFilter, statusFilter);
        }

        private void retrieveRequest(string[] macFilter, string fDateFilter, string tDateFilter, string typeList, string statusFilter)
        {
            Debug.WriteLine("from : " + fDateFilter);
            Debug.WriteLine("to : " + tDateFilter);

            notesVerificationList.Clear();
            FetchDataAsync(HttpRequestHelper.notesRequestString(macFilter, fDateFilter, tDateFilter, typeList, statusFilter), this, "Retrieve Notes");
        }

        public void AsyncResponse(string response, string from)
        {
            if (from == "Retrieve Notes")
            {
                notesVerificationList = new ObservableCollection<NotesVerification>(HttpResponseHelper.retrieveNotesVerificationResponse(response));
                exDevGrid.ItemsSource = notesVerificationList;
                ColumnSetFieldNames();

            }else if(from == "Member") {

                members = HttpResponseHelper.responseMemberList(response);
                App.members = members;
            }
            else
            {
                string[] toSendMembers = members.Where(p => selectedMembers.Contains(p.Key)).Select(x => x.Value).ToArray();
                string toSendTypes = (!selectedType.ToLower().Equals("all")) ? Types[selectedType] : "All";

                retrieveData(toSendMembers, fromDate, toDate, toSendTypes, selectedStatus);
            }
        }

        public void OnSubmit(string fromDate, string toDate, List<string> currentSelectedMembers, string currentSelectedTypes, string currentSelectedStatuses)
        {
            this.selectedMembers = currentSelectedMembers;
            this.selectedType = currentSelectedTypes;
            this.selectedStatus = currentSelectedStatuses;
            this.fromDate = fromDate;
            this.toDate = toDate;

            string[] toSendMembers = members.Where(p => selectedMembers.Contains(p.Key)).Select(x => x.Value).ToArray();
            string toSendTypes = (!selectedType.ToLower().Equals("all")) ? Types[selectedType] : "All";

            retrieveData(toSendMembers, fromDate, toDate, toSendTypes, selectedStatus);

            closeDialog();
        }

        public void OnCancel()
        {
            closeDialog();
        }

        private async void OnFilterClicked()
        {
            await Navigation.PushModalAsync(new VerificationFilterPage(fromDate, toDate, new List<string>(members.Keys), new List<string>(Types.Keys), status, selectedMembers, selectedType, selectedStatus, this), false);
        }

        private void closeDialog()
        {
            Navigation.PopModalAsync(false);
        }

        private void populateType()
        {
            Types.Add("All", "All");
            Types.Add("Sick", "1");
            Types.Add("Allowed Late", "3");
            Types.Add("Paid Leave", "4");
            Types.Add("Early Leave", "5");
            Types.Add("Overtime", "6");
        }

        private void populateStatus()
        {
            status.Add("All");
            status.Add("Unapproved");
            status.Add("Approved");
            status.Add("Rejected");
        }

        public void OnSubmit(string currentSelectedStatuses)
        {
            lastSelectedApproval = currentSelectedStatuses;

            FetchDataAsync(HttpRequestHelper.notesConfirmationString(App.thisDeviceMac, selectedRows, lastSelectedApproval), this, "Retrieve Notes");

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
