using Redux;
using SpotFinder.Config;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Root
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootMasterDetailPage : MasterDetailPage
    {
        public RootMasterDetailPage()
        {
            InitializeComponent();
            MenuMasterDetailPageXaml.ListView.ItemSelected += OnMenuItemSelected;
            IsPresentedChanged += IsPresentedChangedTwo;
        }

        async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                MenuMasterDetailPageXaml.ListView.SelectedItem = null;
                if (App.Current.MainPage.Navigation.NavigationStack.Last().GetType() == typeof(RootMasterDetailPage))
                {
                    if (item.TargetType == typeof(AddingProcessPage))
                        DIContainer.Instance.Resolve<IStore<ApplicationState>>().Dispatch(new SetNewReportAction());

                    await Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType));
                }

                if (Device.RuntimePlatform != Device.WinRT)
                    IsPresented = false;
            }
        }

        public void IsPresentedChangedTwo(object sender, EventArgs e)
        {
            if (IsPresented == true)
                Title = "Spots Finder";
            else
                Title = "";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var customNavigationPage = App.Current.MainPage as CustomNavigationPage;
            if(customNavigationPage != null)
                customNavigationPage.BackgroundColor = (Color)App.Current.Resources["PageBackgroundColor"];
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var customNavigationPage = App.Current.MainPage as CustomNavigationPage;
            if (customNavigationPage != null)
                customNavigationPage.BackgroundColor = (Color)App.Current.Resources["NavigationBarColor"];
        }
    }
}