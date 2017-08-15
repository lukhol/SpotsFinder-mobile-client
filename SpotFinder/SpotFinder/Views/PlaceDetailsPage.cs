using SpotFinder.Core;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder.Views
{
    public partial class PlaceDetailsPage : ContentPage
    {
        public PlaceDetailsPage(Place place, Command command = null, string message = null)
        {
            var placeDetailsViewModel = new PlaceDetailsViewModel(Navigation);
            
            if (command != null)
            {
                NavigationPage.SetHasNavigationBar(this, false);
                placeDetailsViewModel.Initialize(place, command, message);
            }
            else
            {
                NavigationPage.SetHasNavigationBar(this, true);
                placeDetailsViewModel.Initialize(place);
            }

            placeDetailsViewModel.InjectPage(this, "DetailsPage");
            BindingContext = placeDetailsViewModel;
        }

        protected override void OnAppearing()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            base.OnAppearing();
        }
    }
}