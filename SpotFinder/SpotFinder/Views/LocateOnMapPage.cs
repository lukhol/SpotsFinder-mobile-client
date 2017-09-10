using Microsoft.Practices.ServiceLocation;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder.Views
{
    public class LocateOnMapPage : ContentPage
    {
        public LocateOnMapPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Title = "Locate on map page";
            var locateOnMapViewModel = ServiceLocator.Current.GetInstance<LocateOnMapViewModel>();
            BindingContext = locateOnMapViewModel;
            locateOnMapViewModel.InjectPage(this);
        }

        protected override bool OnBackButtonPressed()
        {
            var locateViewModel = (LocateOnMapViewModel)BindingContext;
            return locateViewModel.IsBusy;
        }
    }
}