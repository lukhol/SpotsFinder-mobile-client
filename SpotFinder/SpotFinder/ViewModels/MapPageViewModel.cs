using SpotFinder.Core;
using SpotFinder.Resx;
using SpotFinder.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.ViewModels
{
    public class MapPageViewModel
    {
        private INavigation Navigation { get; }
        private IPlaceRepository PlaceRepository { get; }
        private ILocalPlaceRepository LocalPlaceRepository { get; }
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];
        private ContentPage CurrentPage { get; set; }
        private Map map;

        public MapPageViewModel(INavigation navigation, IPlaceRepository placeRepository, ILocalPlaceRepository localPlaceRepository)
        {
            Navigation = navigation ?? throw new ArgumentNullException("navigation is null in MapPageViewModel");
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in MapPageViewModel");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in MapPageViewModel");
        }

        public void InjectPage(ContentPage contentPage, string pageTitle)
        {
            CurrentPage = contentPage;
            CurrentPage.Title = pageTitle;
            ShowMap();
        }

        public void ShowMap()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];
            CurrentPage.Content = CreateMapLayout();
            if(map.Pins.Count == 0)
                UpdateMapPins();
        }

        public bool UpdateMapPins()
        {
            var allPlaces = LocalPlaceRepository.GetAllPlaces();

            if (allPlaces == null || allPlaces.Count == 0)
                return false;

            map.Pins.Clear();

            foreach (var place in allPlaces)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(place.Location.Latitude, place.Location.Longitude),
                    Label = place.Name + " ",
                    Address = place.Description + " "
                };
                map.Pins.Add(pin);
            }

            return true;
        }

        private StackLayout CreateMapLayout()
        {
            StackLayout layout;

            if(map == null)
                map = new Map(
                MapSpan.FromCenterAndRadius(new Position(51.75924850, 19.45598330), Distance.FromMiles(0.3)))
                {
                    IsShowingUser = true,
                    HeightRequest = 2480,
                    WidthRequest = 960,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

            var addPlaceButton = new Button
            {
                Margin = new Thickness(10, 0, 0, 0),
                BackgroundColor = new Color(24, 178, 40, 0.5),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.AddSpotButton
            };
            addPlaceButton.Command = new Command(() =>
            {
                Navigation.PushAsync(new AddingProcessPage());
            });

            var mapLayout = Utils.CreateItemOnItemLayout(map, addPlaceButton);

            layout = new StackLayout();
            layout.Children.Add(mapLayout);

            return layout;
        }

        public void TestDostaniaSrodkaMapy()
        {
            var visibleRegion = map.VisibleRegion;
            var centerPosition = visibleRegion.Center;
        }
    }
}
