using System;
using Xamarin.Forms;

namespace SpotFinder.Views.Root
{
    public class RootMasterDetailPage : MasterDetailPage
    {
        MenuMasterDetailPageTwo menuMasterDetailPage;

        public RootMasterDetailPage()
        {
            Title = "Spots Finder";

            MasterBehavior = MasterBehavior.Popover;

            menuMasterDetailPage = new MenuMasterDetailPageTwo();

            Master = menuMasterDetailPage;
            Detail = new InfoPage();

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
