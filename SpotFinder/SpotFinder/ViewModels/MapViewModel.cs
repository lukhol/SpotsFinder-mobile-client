﻿using SpotFinder.Models.Core;
using SpotFinder.Redux.Actions;
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
        public MapViewModel()
        {
            App.AppStore
                .DistinctUntilChanged(state => new { state.PlacesData.ListOfPlaces })
                .Subscribe(state =>
                {
                    if (state.PlacesData.ListOfPlaces == null)
                    {
                        IsBusy = true;
                    }
                    else
                    {
                        UpdateMap(state.PlacesData.ListOfPlaces);
                        IsBusy = false;
                    }
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

        public void UpdateMap(List<Place> places)
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
                    App.AppStore.Dispatch(new ClearActuallyShowingPlaceAction());
                    App.AppStore.Dispatch(new RequestDownloadSpotAction(place.Id));

                    await App.Current.MainPage.Navigation.PushAsync(new PlaceDetailsPage());
                };

                pins.Add(pin);
            }

            MapCenterLocation = new Position(places.First().Location.Latitude, places.First().Location.Longitude);

            PinsCollection = pins;
        }

        public Position MapCenterLocation
        {
            get
            {
                var deviceData = App.AppStore.GetState().DeviceData;
                Position myPosition;
                if (deviceData != null && deviceData.Location != null)
                    myPosition = new Position(deviceData.Location.Latitude, deviceData.Location.Longitude);
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
