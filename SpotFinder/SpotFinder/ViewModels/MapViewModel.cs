using Redux;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
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
        private IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator;

        public MapViewModel(IStore<ApplicationState> appStore, IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator) : base(appStore)
        {
            this.downloadPlaceByIdActionCreator = downloadPlaceByIdActionCreator ?? throw new ArgumentNullException(nameof(downloadPlaceByIdActionCreator));

            SetMapTypeFromAppSettings();

            appStore
                .DistinctUntilChanged(state => new { state.PlacesData.PlacesListState.Status })
                .Subscribe(state =>
                {
                    var placesList = state.PlacesData.PlacesListState.Value;
                
                    if (placesList != null && state.PlacesData.PlacesListState.Status == Core.Enums.Status.Success)
                        UpdateMap(placesList);
                    else
                        IsBusy = true;
                });
        }

        private void SetMapTypeFromAppSettings()
        {
            var mapTypeFromSettings = appStore.GetState().Settings.MapType;

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

        private void SetMapPosition()
        {
            var placesList = appStore.GetState().PlacesData.PlacesListState.Value;
            if(placesList != null && placesList.Count > 0)
            {
                var lastSearchingCriteria = appStore.GetState().PlacesData.PlacesListState.TriggerValue;
                if(lastSearchingCriteria != null)
                {
                    if (lastSearchingCriteria.Location.City != null)
                        MapCenterLocation = ComputeCenterPosition();
                    else
                        MapCenterLocation = new Position(
                            (double)lastSearchingCriteria.Location.Latitude, 
                            (double)lastSearchingCriteria.Location.Longitude
                        );

                    return;
                }

                MapCenterLocation = ComputeCenterPosition();
                return;
            }

            var locationStateValue = appStore.GetState().DeviceData.LocationState.Value;
            if (locationStateValue != null)
            {
                MapCenterLocation = new Position(locationStateValue.Latitude, locationStateValue.Longitude);
                return;
            }
        }

        private void UpdateMap(IList<Place> places)
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
                    appStore.DispatchAsync(downloadPlaceByIdActionCreator.GetPlaceById(place.Id));

                    await App.Current.MainPage.Navigation.PushAsync(new PlaceDetailsPage());
                };

                pins.Add(pin);
            }

            SetMapPosition();
            PinsCollection = pins;
            IsBusy = false;
        }

        private Position ComputeCenterPosition()
        {
            var spotsList = appStore.GetState().PlacesData.PlacesListState.Value;

            if (spotsList == null)
                return new Position(0, 0);

            var maxLatitude = spotsList.Max(x => x.Location.Latitude);
            var minLatitude = spotsList.Min(x => x.Location.Latitude);

            var maxLongitude = spotsList.Max(x => x.Location.Longitude);
            var minLongitude = spotsList.Min(x => x.Location.Longitude);

            var averageLatitude = (maxLatitude + minLatitude) / 2;
            var averageLongitude = (maxLongitude + minLongitude) / 2;

            return new Position(averageLatitude, averageLongitude);
        }

        private Position mapCenterLocation;
        public Position MapCenterLocation
        {
            get => mapCenterLocation;
            set
            {
                mapCenterLocation = value;
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
