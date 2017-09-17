using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Root.Xaml
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootMasterDetailPage : MasterDetailPage
    {
        public RootMasterDetailPage()
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