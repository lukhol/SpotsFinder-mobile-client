using System.Collections.Generic;
using Xamarin.Forms;

namespace SpotFinder.Views.Root
{
    public class MenuMasterDetailPage : ContentPage
    {
        public ListView ListView
        {
            get { return listView; }
            set { listView = value; }
        }
        private ListView listView;

        public MenuMasterDetailPage()
        {
            Title = "Menu";
            BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];

            var masterPageItems = new List<MasterPageItem>
            {
                new MasterPageItem
                {
                    Title = "Criteria",
                    TargetType = typeof(CriteriaPage),
                    IconSource = "criteriaIcon.png"
                },
                new MasterPageItem
                {
                    Title = "List",
                    TargetType = typeof(ListPage),
                    IconSource = "listIcon.png"
                },
                new MasterPageItem
                {
                    Title = "Map",
                    TargetType = typeof(MapPage),
                    IconSource = "mapIcon.png"
                },
                new MasterPageItem
                {
                    Title = "Add spot",
                    TargetType = typeof(AddingProcessPage),
                    IconSource = "plusIcon.png"
                },
                new MasterPageItem
                {
                    Title = "Settings",
                    TargetType = typeof(SettingsPage),
                    IconSource = "settingsIcon.png"
                },
                
            };

            listView = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var cell = new ImageCell();
                    cell.SetBinding(ImageCell.TextProperty, "Title");
                    cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
                    cell.TextColor = Color.White;
                    return cell;
                }),
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None,
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(5)
            };

            Content = new StackLayout
            {
                Margin = new Thickness(10, 10, 10, 10),
                Children =
                {
                    listView
                }
            };

            listView.ItemsSource = masterPageItems;
        }
    }
}