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
                placeDetailsViewModel.Initialize(place, command, message);
            }
            else
            {
                placeDetailsViewModel.Initialize(place);
            }

            placeDetailsViewModel.InjectPage(this, "DetailsPage");
            BindingContext = placeDetailsViewModel;
        }

        protected override void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            base.OnAppearing();
        }
    }
}