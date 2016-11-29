
using System;
using Xamarin.Forms;

namespace Attend.Pages
{
    class VerificationApprovalPage : ContentPage
    {
        Picker Statuses = new Picker();
        string selectedStatuses = "";

        private IOnSubmitVApproval Caller = null;

        public interface IOnSubmitVApproval
        {
            void OnSubmit(string currentSelectedStatuses);
            void OnCancel();
        }

        public VerificationApprovalPage(string lastSelected, IOnSubmitVApproval Caller)
        {
            this.Caller = Caller;

            selectedStatuses = lastSelected;

            Statuses.Items.Add("Approved");
            Statuses.Items.Add("Rejected");

            Statuses.SelectedIndex = Statuses.Items.IndexOf(selectedStatuses);
            Statuses.SelectedIndexChanged += (sender, args) =>
            {
                selectedStatuses = Statuses.Items[Statuses.SelectedIndex];
            };

            StackLayout leftFilters = new StackLayout
            {
                Children = {

                    new Label { Text = "Status"},
                    Statuses
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

            leftFilters.Padding = 5;

            Grid.SetColumnSpan(leftFilters, 2);

            WholeContainer.Children.Add(leftFilters, 0, 0);
            WholeContainer.Children.Add(Submit, 0, 1);
            WholeContainer.Children.Add(Cancel, 1, 1);

            WholeContainer.BackgroundColor = Color.Silver;
            WholeContainer.VerticalOptions = LayoutOptions.CenterAndExpand;
            WholeContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;

            Content = WholeContainer;
        }

        private void DialogSubmited(object sender, EventArgs e)
        {
            Caller.OnSubmit(selectedStatuses);
        }

        private void DialogCanceled(object sender, EventArgs e)
        {
            Caller.OnCancel();
        }
    }
}
