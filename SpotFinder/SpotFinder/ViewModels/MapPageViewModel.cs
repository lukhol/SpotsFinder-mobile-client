using SpotFinder.Core;
using SpotFinder.Resx;
using SpotFinder.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.ViewModels
{
    public class MapPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation Navigation { get; }
        private IPlaceRepository PlaceRepository { get; }
        private ILocalPlaceRepository LocalPlaceRepository { get; }
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];
        private ContentPage CurrentPage { get; set; }
        private Map map;
        private IList<Place> allPlaces;

        private StackLayout mainStackLayout;
        private StackLayout loadingStackLayout;

        public MapPageViewModel(INavigation navigation, IPlaceRepository placeRepository, ILocalPlaceRepository localPlaceRepository)
        {
            Navigation = navigation ?? throw new ArgumentNullException("navigation is null in MapPageViewModel");
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in MapPageViewModel");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in MapPageViewModel");

            mainStackLayout = CreateMapLayout();
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
            if (mainStackLayout == null)
                CurrentPage.Content = CreateMapLayout();
            else
                CurrentPage.Content = mainStackLayout;
        }

        private void GetSpots()
        {
            allPlaces = LocalPlaceRepository.GetAllPlaces();
            IsBussy = false;
        }

        public void UpdateMapPins()
        {
            if (allPlaces == null || allPlaces.Count == 0)
                return;

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

                pin.Clicked += (s, e) =>
                {
                    Navigation.PushAsync(new PlaceDetailsPage(place));
                };

                map.Pins.Add(pin);
            }
        }

        private StackLayout CreateMapLayout()
        {
            if (map == null)
                map = new Map(
                MapSpan.FromCenterAndRadius(new Position(51.75924850, 19.45598330), Distance.FromMiles(0.3)))
                {
                    HeightRequest = 2480,
                    WidthRequest = 960,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

            loadingStackLayout = new StackLayout
            {
                Children =
                {
                    new ActivityIndicator
                    {
                        IsVisible = true,
                        IsRunning = true,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.Center,
                        Color = mainAccentColor
                    }
                },
                IsVisible = false,
                BackgroundColor = Color.FromRgba(12, 12, 12, 200)
            };
            loadingStackLayout.SetBinding(StackLayout.IsVisibleProperty, "IsBussy");

            var addPlaceButton = new Button
            {
                Margin = new Thickness(10, 10, 0, 0),
                BackgroundColor = new Color(24, 178, 40, 0.5),
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.AddSpotButton
            };

            var filterButton = new Button
            {
                Margin = new Thickness(10, 0, 0, 0),
                BackgroundColor = new Color(24, 178, 40, 0.5),
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.FilterLabel
            };

            var refreshButton = new Button
            {
                Margin = new Thickness(10, 0, 0, 0),
                BackgroundColor = new Color(24, 178, 40, 0.5),
                HorizontalOptions = LayoutOptions.Start,
                Text = "Refresh"
            };

            addPlaceButton.Command = new Command(() => { Navigation.PushAsync(new AddingProcessPage()); });
            filterButton.Command = new Command(() => { Navigation.PushAsync(new CriteriaPage()); });
            refreshButton.Command = new Command(() => { IsBussy = true; });

            var buttonsLayout = new StackLayout
            {
                Children =
                {
                    addPlaceButton, filterButton, refreshButton
                }
            };

            return Utils.CreateItemOnItemLayout(map, buttonsLayout, loadingStackLayout);
        }

        private bool isBussy = false;
        public bool IsBussy
        {
            get => isBussy;
            set
            {
                isBussy = value;
                OnPropertyChanged();

                if (value == true)
                {
                    Task.Run(() => { GetSpots(); }).ContinueWith((t) => { IsBussy = false; });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UpdateMapPins();
                    });
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
