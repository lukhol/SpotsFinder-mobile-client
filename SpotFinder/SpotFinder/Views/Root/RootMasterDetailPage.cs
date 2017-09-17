using System;
using Xamarin.Forms;

namespace SpotFinder.Views.Root
{
    public class RootMasterDetailPage : MasterDetailPage
    {
        MenuMasterDetailPage menuMasterDetailPage;

        public RootMasterDetailPage()
        {
            Title = "Spots Finder";

            MasterBehavior = MasterBehavior.Popover;

            menuMasterDetailPage = new MenuMasterDetailPage();

            Master = menuMasterDetailPage;
            Detail = new Views.Xaml.InfoPage();

            menuMasterDetailPage.ListView.ItemSelected += OnItemSelected;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if(item != null)
            {
                menuMasterDetailPage.ListView.SelectedItem = null;
                await Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType));

                if (Device.RuntimePlatform != Device.Windows)
                    IsPresented = false;
            }
        }
    }
}
