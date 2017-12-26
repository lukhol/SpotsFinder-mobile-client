using SpotFinder.Models.Core;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Resx;
using SpotFinder.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private IDownloadPlaceByIdActionCreator downloadPlaceByIdActionCreator;

        public MapViewModel(IDownloadPlaceByIdActionCreator downloadPlaceByIdActionCreator)
        {
            this.downloadPlaceByIdActionCreator = downloadPlaceByIdActionCreator ?? throw new ArgumentNullException(nameof(downloadPlaceByIdActionCreator));

            App.AppStore
                .DistinctUntilChanged(state => new { state.PlacesData.PlacesListState.Status })
                .Subscribe(state =>
                {
                    var placesList = state.PlacesData.PlacesListState.Value;
                    if (placesList != null && state.PlacesData.PlacesListState.Status == Core.Enums.Status.Success)
                        UpdateMap(placesList);
                    else
                        IsBusy = true;
                });

            var mapTypeFromSettings = App.AppStore.GetState().Settings.MapType;

            switch (mapTypeFromSettings)
            {
                case Core.Enums.MapType.Normal:
                    mapTypeProperty = MapType.Street;
                    break;

                case Core.Enums.MapType.Satelite:
                    mapTypeProperty = MapType.Satellite;
                    break;

                default:
                    mapTypeProperty = MapType.Street;
                    break;
            }
        }

        public void UpdateMap(IList<Place> places)
        {
            if (places == null || places.Count == 0)
            {
                IsBusy = false;

                pinsCollection.Clear();
                OnPropertyChanged("PinsCollection");

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (App.Current.MainPage.Navigation.NavigationStack.Last().GetType() == typeof(MapPage))
                        App.Current.MainPage.DisplayAlert("Ups", AppResources.CountSpotsInformationNotFound, "Ok");
                });

                return;
            }

            var pins = new ObservableCollection<Pin>();

            foreach (var place in places)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(place.Location.Latitude, place.Location.Longitude),
                    Label = place.Name + " ",
                    Address = place.Description + " "
                };

                pin.Clicked += async (s, e) =>
                {
                    App.AppStore.DispatchAsync(downloadPlaceByIdActionCreator.DownloadPlaceById(place.Id));

                    await App.Current.MainPage.Navigation.PushAsync(new PlaceDetailsPage());
                };

                pins.Add(pin);
            }

            MapCenterLocation = new Position(places.First().Location.Latitude, places.First().Location.Longitude);

            PinsCollection = pins;

            IsBusy = false;
        }

        public Position MapCenterLocation
        {
            get
            {
                var deviceData = App.AppStore.GetState().DeviceData;
                Position myPosition;
                if (deviceData != null && deviceData.LocationState.Value != null)
                    myPosition = new Position(deviceData.LocationState.Value.Latitude, deviceData.LocationState.Value.Longitude);
                else
                    myPosition = new Position(0, 0);
                return myPosition;
            }
            set
            {
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Pin> pinsCollection = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> PinsCollection
        {
            get => pinsCollection;
            set
            {
                pinsCollection = value;
                OnPropertyChanged();
            }
        }

        private MapType mapTypeProperty;
        public MapType MapTypeProperty
        {
            get => mapTypeProperty;
            set
            {
                mapTypeProperty = value;
                OnPropertyChanged();
            }
        }
    }
}
