using Redux;
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
        private IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator;

        public MapViewModel(IStore<ApplicationState> appStore, IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator) : base(appStore)
        {
            this.downloadPlaceByIdActionCreator = downloadPlaceByIdActionCreator ?? throw new ArgumentNullException(nameof(downloadPlaceByIdActionCreator));

            SetMapTypeFromAppSettings();

            appStore
                .DistinctUntilChanged(state => new { state.PlacesData.PlacesListState.Status })
                .SubscribeWithError(state =>
                {
                    var placesList = state.PlacesData.PlacesListState.Value;

                    if (placesList != null && state.PlacesData.PlacesListState.Status == Core.Enums.Status.Success)
                        UpdateMap(placesList);
                    else
                        IsBusy = true;
                }, error => { appStore.Dispatch(new SetErrorAction(error, "MapViewModel in subscription.")); });
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
                    appStore.DispatchAsync(downloadPlaceByIdActionCreator.GetPlaceById(place.Id, place.Version));

                    await App.Current.MainPage.Navigation.PushAsync(new PlaceDetailsPage());
                };

                pins.Add(pin);
            }

            SetMapPosition();
            SetMapSpanFromPositionAndDistance()

            PinsCollection = pins;
            IsBusy = false;
        }

        private void SetMapPosition()
        {
            var placesList = appStore.GetState().PlacesData.PlacesListState.Value;
            if (placesList != null && placesList.Count > 0)
            {
                var lastSearchingCriteria = appStore.GetState().PlacesData.PlacesListState.TriggerValue;
                if (lastSearchingCriteria != null)
                {
                    if (lastSearchingCriteria.Location.City != null)
                        MapCenterPosition = ComputeCenterPosition();
                    else
                        MapCenterPosition = new Position(
                            (double)lastSearchingCriteria.Location.Latitude,
                            (double)lastSearchingCriteria.Location.Longitude
                        );

                    return;
                }

                MapCenterPosition = ComputeCenterPosition();
                return;
            }

            var locationStateValue = appStore.GetState().DeviceData.LocationState.Value;
            if (locationStateValue != null)
            {
                MapCenterPosition = new Position(locationStateValue.Latitude, locationStateValue.Longitude);
                return;
            }
        }

        private void SetMapSpanFromPositionAndDistance()
        {
            var distance = appStore.GetState().PlacesData.PlacesListState.TriggerValue.Distance;
            MapSpan = MapSpan.FromCenterAndRadius(mapCenterPosition, Distance.FromMeters((distance * 1000) * 0.8));
        }

        private Position ComputeCenterPosition()
        {
            var spotsList = appStore.GetState().PlacesData.PlacesListState.Value;

            if (spotsList == null)
                return new Position(0, 0);

            var latitudeAverage = spotsList.Average(x => x.Location.Latitude);
            var longitudeAverage = spotsList.Average(x => x.Location.Longitude);

            var centerLocation = new Location(latitudeAverage, longitudeAverage);

            var nearestSpot = spotsList
                .OrderBy(x => ComputeDistance(centerLocation, x.Location))
                .FirstOrDefault();

            if (nearestSpot == null)
                appStore.Dispatch(new SetErrorAction(new Exception("Computed nearestLocation is null."), nameof(MapViewModel)));

            return new Position(nearestSpot.Location.Latitude, nearestSpot.Location.Longitude);
        }

        private double ComputeDistance(Location centerLocation, Location locationToCompare)
        {
            //Latitude - x, Longitude - y
            var x1 = centerLocation.Latitude;
            var x2 = locationToCompare.Latitude;

            var y1 = centerLocation.Longitude;
            var y2 = locationToCompare.Longitude;

            var distanceSquare = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            return Math.Sqrt(distanceSquare);
        }

        private MapSpan mapSpan;
        public MapSpan MapSpan
        {
            get => mapSpan;
            set
            {
                mapSpan = value;
                OnPropertyChanged();
            }
        }

        private Position mapCenterPosition;
        public Position MapCenterPosition
        {
            get => mapCenterPosition;
            set
            {
                mapCenterPosition = value;
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
