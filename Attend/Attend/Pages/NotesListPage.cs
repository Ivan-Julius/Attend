
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Attend.Entities;
using Attend.Helper;
using Attend.HelperAndroid;
using Xamarin.Forms;
using static Attend.HelperAndroid.HttpClientHelper;
using static Attend.Pages.DateFilterOnlyPage;
using static Attend.Pages.NotesPage;

namespace Attend.Pages
{
    public class NotesListPage : ContentPage, IHttpClient, IOnSubmitDateFilter, IonSubmitNewNotes
    {
        private static string LOG_TAG = typeof(NotesListPage).ToString();
        private ObservableCollection<Notes> MyNotesList = new ObservableCollection<Notes>();

        private string fromDate = "";
        private string toDate = "";

        private ListView notesList = new ListView();
        private Notes selectedNotes = null;

        private StackLayout groupdate = new StackLayout();
        private Grid cellLayout = new Grid();

        int fontSize = 15;
        //int titleFontSize = 15;

        public NotesListPage()
        {
            notesList.ItemTemplate = new DataTemplate(() => { 
                
                Label datesFrom = new Label { FontSize = fontSize};
                datesFrom.SetBinding(Label.TextProperty, new Binding("fromDate"));

                Label datesTo = new Label { FontSize = fontSize };
                datesTo.SetBinding(Label.TextProperty, new Binding("toDate"));

                Label type = new Label { FontSize = fontSize };
                type.SetBinding(Label.TextProperty, new Binding("noteType"));

                Label status = new Label { FontSize = fontSize };
                status.SetBinding(Label.TextProperty, new Binding("status"));

                groupdate = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                        {
                            datesFrom,
                            new Label { Text = " - ", FontSize = fontSize },
                            datesTo
                        },
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    MinimumWidthRequest = 200
                };

                cellLayout = new Grid
                {
                    RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Auto },
                                new RowDefinition { Height = GridLength.Auto },

                        },
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Auto },
                                new ColumnDefinition { Width = GridLength.Auto },
                        }
                };

                cellLayout.BindingContextChanged += CellLayout_BindingContextChanged;

                cellLayout.Children.Add(groupdate, 0, 0);
                cellLayout.Children.Add(type, 0, 1);
                cellLayout.Children.Add(status, 1, 1);

                cellLayout.ColumnSpacing = 50;
                cellLayout.RowSpacing = 15;

                Grid.SetColumnSpan(groupdate, 2);

                cellLayout.Padding = 15;

                var viewAction = new MenuItem { Text = "View" };
                viewAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                viewAction.Clicked += ViewAction_Clicked;

                var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
                deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                deleteAction.Clicked += DeleteAction_Clicked;

                ViewCell customCell = new ViewCell
                {
                    View = cellLayout
                };

                customCell.ContextActions.Add(viewAction);
                customCell.ContextActions.Add(deleteAction);

                customCell.Height = 100;

                return customCell;

            });

            notesList.ItemSelected += NotesList_ItemSelected;
            notesList.RowHeight = 100;
            notesList.ItemsSource = MyNotesList;
            Content = notesList;

            ToolbarItem newNotes = new ToolbarItem { Text = "New" };
            newNotes.Clicked += NewNotes_Clicked;

            ToolbarItem filter = new ToolbarItem { Text = "Filter" };
            filter.Clicked += Filter_Clicked;

            ToolbarItems.Add(newNotes);
            ToolbarItems.Add(filter);

            retrieveData(null, null, null, null, null);
        }

        private void CellLayout_BindingContextChanged(object sender, EventArgs e)
        {
            var sndr = sender as Grid;
            groupdate.BindingContext = sndr.BindingContext;
        }

        private void NotesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedNotes = e.SelectedItem as Notes;
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DateFilterOnlyPage(fromDate, toDate, this), false);
        }

        private async void NewNotes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NotesPage(this, false, null), false);
        }

        private void DeleteAction_Clicked(object sender, EventArgs e)
        {

        }

        private async void ViewAction_Clicked(object sender, EventArgs e)
        {
            try
            {
                var sndr = sender as MenuItem;
                var fromContext = sndr.BindingContext as Notes;
                await Navigation.PushModalAsync(new NotesPage(this, true, fromContext), false);

            }catch(Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
            }
        }

        private void retrieveData(string[] macFilter, string fDateFilter, string tDateFilter, string typeFilter, string statusFilter)
        {
            DateTime fDates = DateTime.Now, tDates = DateTime.Now;

            DateTime today = DateTime.Now;
            DateTime firstDateOfYear = new DateTime(today.Year, 1, 1);

            string remoteSourceDateFormat = DateFormatingHelper.remoteSourceDateFormat;

            if (macFilter == null) macFilter = new string[] { App.thisDeviceMac };

            fromDate = string.IsNullOrEmpty(fDateFilter) ?
                                    firstDateOfYear.ToString(remoteSourceDateFormat) : fDateFilter;

            toDate = string.IsNullOrEmpty(tDateFilter) ?
                                    today.ToString(remoteSourceDateFormat) : tDateFilter;

            typeFilter = (string.IsNullOrEmpty(typeFilter)) ? "All" : typeFilter;
            statusFilter = (string.IsNullOrEmpty(statusFilter)) ? "All" : statusFilter;

            retrieveRequest(macFilter, fromDate, toDate, typeFilter, statusFilter);
        }

        private void retrieveRequest(string[] macFilter, string fDateFilter, string tDateFilter, string typeList, string statusFilter)
        {
            Debug.WriteLine("from : " + fDateFilter);
            Debug.WriteLine("to : " + tDateFilter);

            MyNotesList.Clear();
            FetchDataAsync(HttpRequestHelper.notesRequestString(macFilter, fDateFilter, tDateFilter, typeList, statusFilter), this, "Notes List");
        }


        public void AsyncResponse(string response, string from)
        {
            if (from == "Notes List")
            {
                MyNotesList = new ObservableCollection<Notes>(HttpResponseHelper.retrieveNotesResponse(response));
                notesList.ItemsSource = MyNotesList;

            } else
            {
                retrieveData(null, fromDate, toDate, null, null);
            }
        }

        public void OnCancel()
        {
            closeDialog();
        }

        public void OnSubmit(string fromDate, string toDate)
        {
            this.fromDate = fromDate;
            this.toDate = toDate;

            retrieveData(null, fromDate, toDate, null, null);

            closeDialog();
        }

        public void OnSubmit(Dictionary<string,string> notesTosubmit, double latitude, double longitude)
        {
            FetchDataAsync(
               HttpRequestHelper.notesSubmissionString(
               WifiConnectivityHelper.getMacMarshmellow(), notesTosubmit,
               longitude.ToString(), latitude.ToString()), this, "Notes");

            closeDialog();
        }

        private void closeDialog()
        {
            Navigation.PopModalAsync(false);
        }
    }


}
