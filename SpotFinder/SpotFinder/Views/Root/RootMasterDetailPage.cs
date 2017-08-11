using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.Views.Root
{
    public class RootMasterDetailPage : MasterDetailPage
    {
        MenuMasterDetailPage menuMasterDetailPage;

        public RootMasterDetailPage()
        {
            Application.Current.Resources["UnityContainer"] = new UnityConfig(Navigation);

            Title = "Spots Finder";

            MasterBehavior = MasterBehavior.Popover;

            menuMasterDetailPage = new MenuMasterDetailPage();

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
                IsPresented = false;
            }
        }
    }
}
