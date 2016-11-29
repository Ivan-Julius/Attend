
using System;
using System.Collections.Generic;
using System.Linq;
using Attend.Helper;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace Attend.Pages
{
    public class VerificationFilterPage : ContentPage
    {
        private StackLayout Members = new StackLayout();
        private Picker Statuses = new Picker();
        private Picker Types = new Picker();
        private DatePicker from = null;
        private DatePicker to = null;

        private List<string> MembersList = new List<string>();
        private List<int> selectedMembers = new List<int>();
        private string selectedTypes = "";
        private string selectedStatuses = "";

        private int state = 0;

        private IOnSubmitVFilter Caller = null;

        public interface IOnSubmitVFilter
        {
            void OnSubmit(string fromDate, string toDate, List<string> currentSelectedMembers, string currentSelectedTypes, string currentSelectedStatuses);
            void OnCancel();
        }

        public VerificationFilterPage(string fromDate, string toDate, List<string> MembersList, List<string> TypesList, List<string> StatusesList,
            List<string> formerSelectedMembers, string formerselectedTypes, string formerSelectedStatuses,
            IOnSubmitVFilter Caller)
        {
            this.MembersList = MembersList;
            selectedTypes = formerselectedTypes;
            selectedStatuses = formerSelectedStatuses;
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

            foreach(string a in TypesList)
            {
                Types.Items.Add(a);
            }
            Types.SelectedIndex = Types.Items.IndexOf(selectedTypes);
            Types.SelectedIndexChanged += (sender, args) =>
            {
                selectedTypes = Types.Items[Types.SelectedIndex];
            };

            foreach (string a in StatusesList)
            {
                Statuses.Items.Add(a);
            }
            Statuses.SelectedIndex = Statuses.Items.IndexOf(selectedStatuses);
            Statuses.SelectedIndexChanged += (sender, args) =>
            {
                selectedStatuses = Statuses.Items[Statuses.SelectedIndex];
            };

            StackLayout leftFilters = new StackLayout
            {
                Children = {
                    new Label { Text = "From" },
                    from,
                    new Label { Text = "To" },
                    to,
                    new Label { Text = "Types"},
                    Types,
                    new Label { Text = "Status"},
                    Statuses
                },

                BackgroundColor = Color.Teal,
                Orientation = StackOrientation.Vertical
            };
          
            PopulateList(Members, MembersList);
            
            ScrollView memberList = new ScrollView { Content = Members, Orientation = ScrollOrientation.Vertical, BackgroundColor = Color.Aqua };
      
            memberList.HorizontalOptions = LayoutOptions.CenterAndExpand;

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

            WholeContainer.Children.Add(leftFilters, 0, 0);
            WholeContainer.Children.Add(memberList, 1, 0);
            WholeContainer.Children.Add(Submit, 0, 1);
            WholeContainer.Children.Add(Cancel, 1, 1);

            WholeContainer.BackgroundColor = Color.Silver;
            WholeContainer.VerticalOptions = LayoutOptions.CenterAndExpand;
            WholeContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;

            if (formerSelectedMembers.Count == MembersList.Count)
            {
                ((CheckBox)Members.Children[0]).Checked = true;
            }
            else
            {
                foreach (string a in formerSelectedMembers)
                {
                    if (MembersList.Contains(a))
                        ((CheckBox)Members.Children[MembersList.IndexOf(a) + 1]).Checked = true;
                }
            }

            Content = WholeContainer;
        }

        private void PopulateList(StackLayout layout, List<string> data)
        {         
            layout.Children.Add(newHeaderCheckBox());

            foreach (string d in data)
            {
                layout.Children.Add(newCheckbox(d));
            }
        }

        private CheckBox newCheckbox(string text)
        {
            CheckBox cx = new CheckBox();
            cx.DefaultText = text;
            cx.CheckedChanged += CxItem_CheckedChanged;
           
            return cx;
        }

        private CheckBox newHeaderCheckBox()
        {
            CheckBox cx = new CheckBox();
            cx.DefaultText = "ALL";
            cx.BackgroundColor = Color.Aqua;
            cx.CheckedChanged += CxHeader_CheckedChanged; 
         
            return cx;
        }

        private void CxItem_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            int position = Members.Children.IndexOf(sender as CheckBox) - 1;

            if(selectedMembers.Contains(position)) selectedMembers.Remove(position);
            if (e.Value)
            {
                selectedMembers.Add(position);
            }

            if (state == 0)
            {
                state = 1;
                ((CheckBox)Members.Children[0]).Checked  = (selectedMembers.Count == MembersList.Count)? true : false;

                state = 0;
            }
        }

        private void CxHeader_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (state == 0)
            {
                state = 2;
                selectedMembers.Clear();
                foreach (View child in Members.Children)
                {
                    if(Members.Children.IndexOf(child) > 0) 
                    ((CheckBox)child).Checked = e.Value;
                }

                state = 0;
            }        
        }

        private void DialogSubmited(object sender, EventArgs e)
        {
            Caller.OnSubmit(from.Date.ToString(DateFormatingHelper.remoteSourceDateFormat), to.Date.ToString(DateFormatingHelper.remoteSourceDateFormat),
                MembersList.Where(P => selectedMembers.Contains(MembersList.IndexOf(P))).ToList(), selectedTypes, selectedStatuses);
        }

        private void DialogCanceled(object sender, EventArgs e)
        {
            Caller.OnCancel();
        }
    }
}
