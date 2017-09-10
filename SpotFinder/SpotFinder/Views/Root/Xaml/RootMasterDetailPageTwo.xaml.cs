using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Root
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootMasterDetailPageTwo : MasterDetailPage
    {
        public RootMasterDetailPageTwo()
        {
            InitializeComponent();
            MenuMasterDetailPage.ListView.ItemSelected += OnMenuItemSelected;
        }

        async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                MenuMasterDetailPage.ListView.SelectedItem = null;
                await Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType));

                if (Device.RuntimePlatform != Device.Windows)
                    IsPresented = false;
            }
        }
    }
}