
using Xamarin.Forms;
using System;
using Attend.Pages;
using System.Collections.ObjectModel;

namespace Attend
{
    public class SideMenu : MasterDetailPage
    {
        private ListView listView = new ListView();
        ObservableCollection<MasterItemPage> menus = new ObservableCollection<MasterItemPage>();
        public SideMenu()
        {
            menus.Add(new MasterItemPage { Title = "Attend", targetType = typeof(AttendPage)});
            menus.Add(new MasterItemPage { Title = "Notes", targetType = typeof(NotesListPage)});
            menus.Add(new MasterItemPage { Title = "Daily", targetType = typeof(DailyReportsPage)});
            menus.Add(new MasterItemPage { Title = "Monthly", targetType = typeof(MonthlyReportsPage)});

            //if(App.members.Count > 0)
            menus.Add(new MasterItemPage { Title = "Notes Verification", targetType = typeof(NotesVerificationsPage)});

            listView = new ListView
            {
                ItemsSource = menus,
                ItemTemplate = new DataTemplate(() => {
                    var imageCell = new ImageCell();
                    imageCell.SetBinding(TextCell.TextProperty, "Title");
                    return imageCell;
                }),
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.Default,
                HasUnevenRows = true
            };

            listView.ItemSelected += OnItemSelected;

            // Create the master page with the ListView.
            this.Master = new ContentPage
            {
                Title = "Menu",
                Icon = "Icon.jpg",
                Content = listView
            };
            Master.Opacity = 60;
            listView.SelectedItem = menus[0];
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterItemPage;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.targetType));
                listView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }

    public class MasterItemPage
    {
        public string Title { get; set; }
        public Type targetType { get; set; }
    }

}
