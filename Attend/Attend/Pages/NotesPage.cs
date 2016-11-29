
using System;
using System.Collections.Generic;
using System.Globalization;
using Attend.Entities;
using Attend.ExportedInterface;
using Attend.Helper;
using Xamarin.Forms;

namespace Attend.Pages
{
    public class NotesPage : ContentPage
    {
        private static string LOG_TAG = typeof(ContentPage).ToString();
        private Dictionary<string, int> noteTypes = new Dictionary<string, int>();

        Label fromTimeLabel;
        Grid dateContainer;
        Grid timeContainer;
        Button Reset;
        Button Submit;
        Entry reason;
        Picker notesType;
        DatePicker fromDate;
        DatePicker toDate;
        TimePicker fromTime;
        TimePicker toTime;

        private double latitude = 0.0;
        private double longitude = 0.0;

        int contentCount = 4;

        IonSubmitNewNotes caller;

        public interface IonSubmitNewNotes
        {
            void OnSubmit(Dictionary <string, string> notesToSubmit, double latitude, double longitude);

            void OnCancel();
        }

        public NotesPage(IonSubmitNewNotes caller, bool isReadonly, Notes note)
        {
            this.caller = caller;

            DependencyService.Get<ILocationObtainer>().bindLocationService();

            #region NoteType
            populateNotes();

            notesType = new Picker
            {
                Title = "Note Type",
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Aqua
            };

            foreach (string i in noteTypes.Keys)
            {
                notesType.Items.Add(i);
            }

            notesType.SelectedIndexChanged += delegate
            {
                onSelectedNoteType(notesType.Items[notesType.SelectedIndex]);
            };
            #endregion
           
            Label fromLabel = new Label {
                Text = "From Date",
                FontSize = 18
            };

            Label toLabel = new Label
            {
                Text = "To Date",
                FontSize = 18
            };

            fromDate = new DatePicker { };
            toDate = new DatePicker { };

            fromTimeLabel = new Label
            {
                Text = "From Time",
                FontSize = 18
            };

            Label toTimeLabel = new Label
            {
                Text = "To Time",
                FontSize = 18
            };

            dateContainer = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Yellow,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                      new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                     new ColumnDefinition { Width = GridLength.Auto }
                },
                Padding = 10
            };

            dateContainer.Children.Add(fromLabel, 0, 0);
            dateContainer.Children.Add(toLabel, 1, 0);

            dateContainer.Children.Add(fromDate, 0, 1);
            dateContainer.Children.Add(toDate, 1, 1);

            fromTime = new TimePicker { };
            toTime = new TimePicker { };

            timeContainer = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Silver,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                      new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                     new ColumnDefinition { Width = GridLength.Auto }
                },
                Padding = 10
            };

            timeContainer.Children.Add(fromTimeLabel, 0, 0);
            timeContainer.Children.Add(toTimeLabel, 1, 0);

            timeContainer.Children.Add(fromTime, 0, 1);
            timeContainer.Children.Add(toTime, 1, 1);

            Label reasonsLabel = new Label
            {
                Text = "Reasons",
                FontSize = 18,
                BackgroundColor = Color.Fuchsia
            };

            reason = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Olive,
            };

            Reset = new Button
            {
                Text = "Reset"
                , BackgroundColor = Color.Maroon
            };

            Reset.Clicked += OnReset;

            Submit = new Button
            {
                Text = "Submit"
                , BackgroundColor = Color.Navy
            };

            Submit.Clicked += OnSubmit;

            StackLayout buttons = new StackLayout();
            buttons.Orientation = StackOrientation.Horizontal;
            buttons.Children.Add(Reset);
            buttons.Children.Add(Submit);

            StackLayout absolute = new StackLayout();

            absolute.Orientation = StackOrientation.Vertical;

            absolute.Children.Add(notesType);
            absolute.Children.Add(dateContainer);
            absolute.Children.Add(timeContainer);
            absolute.Children.Add(reasonsLabel);
            absolute.Children.Add(reason);
            absolute.Children.Add(buttons);

            Content = absolute;
           

            if (isReadonly)
            {
                notesType.IsEnabled = false;
                reason.IsEnabled = false;
                fromDate.IsEnabled = false;
                toDate.IsEnabled = false;

                reason.Text = note.reason;
                notesType.SelectedIndex = notesType.Items.IndexOf(note.noteType);
                
                if(notesType.Items.IndexOf(note.noteType) != 2 || notesType.Items.IndexOf(note.noteType) != 5)
                {
                    //time here
                    fromTime.Time = (DateTime.Parse(note.fromDate)).TimeOfDay;
                    toTime.Time = (DateTime.Parse(note.toDate)).TimeOfDay;
                    fromTime.IsEnabled = false;
                    toTime.IsEnabled = false;
                }

                fromDate.Date = DateTime.Parse(note.fromDate, CultureInfo.InvariantCulture);
                toDate.Date = DateTime.Parse(note.toDate, CultureInfo.InvariantCulture);

            }
            else
            {
                notesType.SelectedIndex = 0;
            }
        }

        public void onSelectedNoteType(string selectedType)
        {
            switch (noteTypes[selectedType])
            {
                case 2: 
                case 5: timeContainer.IsVisible = true; contentCount = 4;  break;
                default:
                    timeContainer.IsVisible = false; contentCount = 6;
                    break;
            }
        }

        public void populateNotes()
        {
            if (!noteTypes.ContainsKey("Sick")) noteTypes.Add("Sick", 1);
            if (!noteTypes.ContainsKey("Allowed Late")) noteTypes.Add("Allowed Late", 2);
            if (!noteTypes.ContainsKey("Paid Leave")) noteTypes.Add("Paid Leave", 3);
            if (!noteTypes.ContainsKey("Excused")) noteTypes.Add("Excused", 4);
            if (!noteTypes.ContainsKey("Early")) noteTypes.Add("Early", 5);
            if (!noteTypes.ContainsKey("Overtime")) noteTypes.Add("Overtime", 8);         
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            if (CheckFormIsValid())
            {
                Dictionary<string, string> setUpToSend = new Dictionary<string, string>();
                setUpToSend = submissionPreparations();

                DependencyService.Get<ILocationObtainer>().locationAvailablity(out latitude, out longitude);

                caller.OnSubmit(setUpToSend, latitude, longitude);
            }else
            {
                DisplayAlert("!", "Please make sure all fields are correct & not blank", "Thank You");
            }
        }

        private void OnReset(object sender, EventArgs e)
        {
            notesType.SelectedIndex = 0;
            fromDate.ClearValue(DatePicker.DateProperty);
            toDate.ClearValue(DatePicker.DateProperty);
            reason.Text = "";

            if (contentCount > 4)
            {
                fromTime.ClearValue(TimePicker.TimeProperty);
                toTime.ClearValue(TimePicker.TimeProperty);
            }
        }

        private Dictionary<string, string> submissionPreparations()
        {
            Dictionary<string, string> setUpToSend = new Dictionary<string, string>();

            setUpToSend.Add("Reason_Type", noteTypes[notesType.Items[notesType.SelectedIndex]].ToString());
            setUpToSend.Add("From_Date", (contentCount > 4) ? fromDate.Date.ToString(DateFormatingHelper.YearInFrontFormat) : fromDate.Date.ToString(DateFormatingHelper.YearInFrontFormat).Replace("00:00:00", fromTime.Time.ToString()));
            setUpToSend.Add("To_Date", (contentCount > 4) ? toDate.Date.ToString(DateFormatingHelper.YearInFrontFormat) : toDate.Date.ToString(DateFormatingHelper.YearInFrontFormat).Replace("00:00:00", toTime.Time.ToString()));
            setUpToSend.Add("Reason", reason.Text);

            return setUpToSend;
        }

        private bool CheckFormIsValid()
        {
            return (!string.IsNullOrEmpty(reason.Text));
        }

    }
}
