using SpotFinder.Core;
using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.Repositories;
using SpotFinder.ViewModels;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder.Views
{
    public partial class PlaceDetailsPage : ContentPage
    {
        public PlaceDetailsPage(Place place, Command command = null, string message = null)
        {
            var placeDetailsViewModel = new PlaceDetailsViewModel(new PlaceService(), new LocalPlaceRepository());

            placeDetailsViewModel.InjectPage(this, "DetailsPage");

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

            BindingContext = placeDetailsViewModel;
        }
    }
}