using System;
using Attend.Helper;
using Xamarin.Forms;

namespace Attend.Pages
{
    class DateFilterOnlyPage : ContentPage
    {
        DatePicker from = null;
        DatePicker to = null;

        private IOnSubmitDateFilter Caller = null;

        public interface IOnSubmitDateFilter
        {
            void OnSubmit(string fromDate, string toDate);
            void OnCancel();
        }

        public DateFilterOnlyPage(string fromDate, string toDate, IOnSubmitDateFilter Caller)
        {

            this.Caller = Caller;

            from = new DatePicker();
            if (!string.IsNullOrEmpty(fromDate))
            {
                from.Date = DateTime.Parse(fromDate);
                from.Format = DateFormatingHelper.remoteSourceDateFormat;
            }

            to = new DatePicker();
            if (!string.IsNullOrEmpty(toDate))
            {
                to.Date = DateTime.Parse(toDate);
                to.MinimumDate = DateTime.Parse(fromDate);
                to.Format = DateFormatingHelper.remoteSourceDateFormat;
            }

            StackLayout dateFilters = new StackLayout
            {
                Children = {
                    new Label { Text = "From" },
                    from,
                    new Label { Text = "To" },
                    to
                },

                BackgroundColor = Color.Teal,
                Orientation = StackOrientation.Vertical
            };

           

            Grid WholeContainer = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                     new RowDefinition { Height = GridLength.Auto },
                     
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                     new ColumnDefinition { Width = GridLength.Auto },
                     
                }
            };

            Button Submit = new Button
            {
                Text = "Submit",
                BackgroundColor = Color.Lime
            };

            Submit.Clicked += DialogSubmited;

            Button Cancel = new Button
            {
                Text = "Cancel",
                BackgroundColor = Color.Pink
            };

            Cancel.Clicked += DialogCanceled;

            dateFilters.Padding = 5;

            WholeContainer.Children.Add(dateFilters, 0, 0);
            WholeContainer.Children.Add(Submit, 0, 1);
            WholeContainer.Children.Add(Cancel, 1, 1);


            Grid.SetColumnSpan(dateFilters, 2);

            WholeContainer.BackgroundColor = Color.Silver;
            WholeContainer.VerticalOptions = LayoutOptions.CenterAndExpand;
            WholeContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;

            Content = WholeContainer;
        }

        private void DialogSubmited(object sender, EventArgs e)
        {
            Caller.OnSubmit(from.Date.ToString(DateFormatingHelper.remoteSourceDateFormat), to.Date.ToString(DateFormatingHelper.remoteSourceDateFormat));            
        }

        private void DialogCanceled(object sender, EventArgs e)
        {
            Caller.OnCancel();
        }
    }
}
