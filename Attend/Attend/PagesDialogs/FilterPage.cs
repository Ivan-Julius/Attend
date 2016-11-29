
using System;
using System.Collections.Generic;
using System.Linq;
using Attend.Helper;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace Attend.Pages
{
    public class FilterPage : ContentPage
    {
        StackLayout Members = new StackLayout();
        StackLayout Types = new StackLayout();
        StackLayout Statuses = new StackLayout();

        List<int> selectedMembers = new List<int>();
        List<int> selectedTypes = new List<int>();
        List<int> selectedStatuses = new List<int>();

        List<string> MembersList = new List<string>();
        List<string> TypesList = new List<string>();
        List<string> StatusesList = new List<string>();

        private string StatusLayoutID = "";
        private string MemberLayoutID = "";
        private string TypeLayoutID = "";

        private int MemberState = 0;
        private int TypeState = 0;

        DatePicker from = null;
        DatePicker to = null;

        private IOnSubmit Caller = null;

        public interface IOnSubmit{

            void OnSubmit(string fromDate, string toDate, List<string> currentSelectedMembers, List<string> currentSelectedTypes, List<string> currentSelectedStatuses);
            void OnCancel();
        }

        public FilterPage(string fromDate, string toDate, List<string> MembersList, List<string> TypesList, List<string> StatusesList, 
            List<string> formerSelectedMembers, List<string> formerselectedTypes, List<string> formerSelectedStatuses, 
            bool withTypes, bool withStatus, IOnSubmit Caller)
        {
            this.MembersList = MembersList;

            if(withTypes)
            this.TypesList = TypesList;

            if(withStatus)
            this.StatusesList = StatusesList;

            this.Caller = Caller;

            selectedMembers.Clear();
            selectedTypes.Clear();
            selectedStatuses.Clear();

            from = new DatePicker();
            if(!string.IsNullOrEmpty(fromDate))
            {
                from.Date = DateTime.Parse(fromDate);
                from.Format = DateFormatingHelper.remoteSourceDateFormat;
            }

            to = new DatePicker();
            if(!string.IsNullOrEmpty(toDate))
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

            MemberLayoutID = Members.Id.ToString();
            StatusLayoutID = Statuses.Id.ToString();
            TypeLayoutID = Types.Id.ToString();

            PopulateList(Members, MembersList);
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

            if (withStatus)
            {
                PopulateList(Statuses, StatusesList);
                foreach (string a in formerSelectedStatuses)
                {
                    if (StatusesList.Contains(a))
                        ((CheckBox)Statuses.Children[StatusesList.IndexOf(a) + 1]).Checked = true;
                }
            }

            if (withTypes)
            {
                PopulateList(Types, TypesList);
                if (formerselectedTypes.Count == TypesList.Count)
                {
                    ((CheckBox)Types.Children[0]).Checked = true;
                }
                else
                {
                    foreach (string a in formerselectedTypes)
                    {
                        if (TypesList.Contains(a))
                            ((CheckBox)Types.Children[TypesList.IndexOf(a) + 1]).Checked = true;
                    }
                }
            }

            ScrollView statusList = new ScrollView { Content = Statuses, Orientation = ScrollOrientation.Vertical, BackgroundColor = Color.Olive };
            ScrollView memberList = new ScrollView { Content = Members, Orientation = ScrollOrientation.Vertical, BackgroundColor = Color.Aqua};
            ScrollView typeList = new ScrollView { Content = Types, Orientation = ScrollOrientation.Vertical, BackgroundColor = Color.Lime };

            typeList.HorizontalOptions = LayoutOptions.CenterAndExpand;
            memberList.HorizontalOptions = LayoutOptions.CenterAndExpand;

            Grid WholeContainer = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                     new RowDefinition { Height = GridLength.Auto },
                      new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                     new ColumnDefinition { Width = GridLength.Auto },
                       new ColumnDefinition { Width = GridLength.Auto },
                }
            };

            Button Submit = new Button {
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

            if(withStatus)
            WholeContainer.Children.Add(statusList, 0, 1);

            WholeContainer.Children.Add(memberList, 1, 0);

            if(withTypes)
            WholeContainer.Children.Add(typeList, 2, 0);

            if (withStatus)
            {
                WholeContainer.Children.Add(Submit, 1, 2);
                WholeContainer.Children.Add(Cancel, 2, 2);
            }else
            {
                WholeContainer.Children.Add(Submit, 0, 2);
                WholeContainer.Children.Add(Cancel, 1, 2);
            }

            if(withTypes)
            Grid.SetRowSpan(typeList, 2);

            if (withStatus)
            {
                Grid.SetRowSpan(statusList, 2);

            }else
            {
                Grid.SetRowSpan(dateFilters, 2);
            }

            Grid.SetRowSpan(memberList, 2);

            WholeContainer.BackgroundColor = Color.Silver;
            WholeContainer.VerticalOptions = LayoutOptions.CenterAndExpand;
            WholeContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;

            Content = WholeContainer;
        }

        private void PopulateList(StackLayout layout, List<string> data)
        {
            if(!layout.Id.ToString().ToLower().Equals(StatusLayoutID.ToLower()))
            layout.Children.Add(newHeaderCheckBox());

            foreach(string d in data)
            {
                layout.Children.Add(newCheckbox(d));
            }
        }

        private CheckBox newCheckbox(string text)
        {
            int defaultState = 99;

            CheckBox cx = new CheckBox();
            cx.DefaultText = text;
            cx.CheckedChanged += (object sender, XLabs.EventArgs<bool> e) =>
            {
                string type = (sender as CheckBox).Parent.Id.ToString();
                if (type.ToLower().Equals(MemberLayoutID.ToLower()))
                {
                    CheckedItem(sender as CheckBox, e.Value, Members, ref selectedMembers, ref MembersList, ref MemberState);
                }
                else if (type.ToLower().Equals(TypeLayoutID.ToLower()))
                {
                    CheckedItem(sender as CheckBox, e.Value, Types, ref selectedTypes, ref TypesList, ref TypeState);
                }
                else
                {
                    CheckedItem(sender as CheckBox, e.Value, Statuses, ref selectedStatuses, ref StatusesList, ref defaultState);
                }
            };
        
            return cx;
        }

        private CheckBox newHeaderCheckBox()
        {
            int defaultState = 99;

            CheckBox cx = new CheckBox();
            cx.DefaultText = "ALL";
            cx.BackgroundColor = Color.Aqua;
            cx.CheckedChanged += (object sender, XLabs.EventArgs<bool> e) =>
            {
                string type = (sender as CheckBox).Parent.Id.ToString();

                if (type.ToLower().Equals(MemberLayoutID.ToLower()))
                {
                    CheckedHeaderItem(sender as CheckBox, e.Value, Members, ref selectedMembers, ref MemberState);
                }
                else if (type.ToLower().Equals(TypeLayoutID.ToLower()))
                {
                    CheckedHeaderItem(sender as CheckBox, e.Value, Types, ref selectedTypes, ref TypeState);
                }
                else
                {
                    CheckedHeaderItem(sender as CheckBox, e.Value, Statuses, ref selectedStatuses, ref defaultState);
                }
            };
            
            return cx;
        }

        private void CheckedItem(CheckBox sender, bool isChecked, StackLayout type, ref List<int> Selectionlist, ref List<string> ContainerList, ref int State)
        {

            bool condition = (type.Id.ToString().ToLower().Equals(StatusLayoutID.ToLower()));
            int position = (condition)? type.Children.IndexOf(sender) : type.Children.IndexOf(sender) - 1;

            if (Selectionlist.Contains(position)) Selectionlist.Remove(position);
            if (isChecked) Selectionlist.Add(position);

            if (State == 0 && !condition)
            {
                State = 1;
                ((CheckBox)type.Children[0]).Checked = (Selectionlist.Count == ContainerList.Count) ? true : false;

                State = 0;
            }
        }

        private void CheckedHeaderItem(CheckBox sender, bool isChecked, StackLayout type, ref List<int> Selectionlist, ref int State)
        {
            bool condition = (type.Id.ToString().ToLower().Equals(StatusLayoutID.ToLower()));

            if (State == 0 && !condition)
            {
                State = 2;
                Selectionlist.Clear();
                foreach (View child in type.Children)
                {
                    if (type.Children.IndexOf(child) > 0)
                        ((CheckBox)child).Checked = isChecked;
                }

                State = 0;
            }
        }

        private void DialogSubmited(object sender, EventArgs e)
        { 
            Caller.OnSubmit(from.Date.ToString(DateFormatingHelper.remoteSourceDateFormat), to.Date.ToString(DateFormatingHelper.remoteSourceDateFormat),
                 MembersList.Where(P => selectedMembers.Contains(MembersList.IndexOf(P))).ToList(),
                   TypesList.Where(P => selectedTypes.Contains(TypesList.IndexOf(P))).ToList(),
                   StatusesList.Where(P => selectedStatuses.Contains(StatusesList.IndexOf(P))).ToList());
        }

        private void DialogCanceled(object sender, EventArgs e)
        {
            Caller.OnCancel();
        }
    }
}
