using SpotFinder.Core;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
using SpotFinder.ViewModels.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Xaml
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceDetailsPage : ContentPage
    {
        private Place place;

        public PlaceDetailsPage()
        {
            InitializeComponent();
            BindingContext = new PlaceDetailsViewModel();

            App.AppStore.GetState().PlaceDownloaded += Update;
            App.AppStore.Dispatch(new DownloadSinglePlaceAction(26));  

            PreapreListItemSelected();
        }

        public PlaceDetailsPage(Place place)
        {
            InitializeComponent();
            BindingContext = new PlaceDetailsViewModel(place);

            this.place = place;

            PreapreListItemSelected();
            Update();
        }

        public void Update()
        {
            if(place == null)
                place = App.AppStore.GetState().ShowingPlace;

            if (place != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(place.Location.Latitude, place.Location.Longitude), Distance.FromMiles(0.3)));

                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(place.Location.Latitude, place.Location.Longitude),
                        Label = place.Name + " ",
                        Address = place.Description + " "
                    };

                    MyMap.Pins.Add(pin);

                    foreach (var item in place.PhotosBase64)
                    {
                        var image = new Image();
                        image.Source = Utils.Base64ImageToImageSource(item);
                            ImagesStackLayout.Children.Add(image);
                    }

                    var viewModel = (PlaceDetailsViewModel)BindingContext;
                    viewModel.Place = place;
                });
            }
        }

        private void PreapreListItemSelected()
        {
            ObstacleListView.ItemSelected += (s, e) =>
            {
                ObstacleListView.SelectedItem = null;
            };
        }
    }
}