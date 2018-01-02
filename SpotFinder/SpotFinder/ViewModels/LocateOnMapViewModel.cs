﻿using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using SpotFinder.Redux.Actions;
using SpotFinder.Repositories;
using SpotFinder.DataServices;
using System;
using SpotFinder.Redux;
using Redux;
using SpotFinder.OwnControls;
using SpotFinder.Models.Core;
using SpotFinder.Resx;
using System.Reactive.Linq;
using SpotFinder.Exceptions;

namespace SpotFinder.ViewModels
{
    public class LocateOnMapViewModel : BaseViewModel
    {
        private IPlaceService PlaceService { get; }
        private IPlaceRepository LocalPlaceRepository { get; }

        public LocateOnMapViewModel(IStore<ApplicationState> appStore, 
            IPlaceService placeService, IPlaceRepository localPlaceRepository) : base(appStore)
        {
            PlaceService = placeService ?? throw new ArgumentNullException("PlaceService is null in LocateOnMapViewModel");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("LocalPlaceRepository is null in LocateOnMapViewModel");

            appStore
                .DistinctUntilChanged(state => new { state.DeviceData.LocationState.Status })
                .Subscribe(state =>
                {
                    var stateLocation = appStore.GetState().DeviceData.LocationState.Value;
                    mapCenterLocation = new Position(stateLocation.Latitude, stateLocation.Longitude);
                }, error => { appStore.Dispatch(new SetErrorAction(error, "LocateOnMapViewModel - subscription.")); });

            var location = appStore.GetState().DeviceData.LocationState.Value;


            SetMapTypeFromSettings();
        }

        private void SetMapTypeFromSettings()
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

        private string busyMessage = AppResources.UploadingPrompt;
        public string BusyMessage
        {
            get => busyMessage;
            set
            {
                busyMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand UploadPlaceCommand => new Command(Upload);

        private async void Upload(object param)
        {
            var map = param as BindableMap;

            if (map == null)
                return;

            IsBusy = true;

            var mapLocation = new Location(map.VisibleRegion.Center.Latitude, map.VisibleRegion.Center.Longitude);

            appStore.Dispatch(new SetReportLocationAction(mapLocation));
            var addingPlace = appStore.GetState().PlacesData.ReportState.Value.Place;

            int result = await PlaceService.SendAsync(addingPlace);

            if (result == 0)
            {
                var alertResult = await App.Current.MainPage.DisplayAlert(
                    "Error.", 
                    "Problem with publishing spot. Try again or add to offline list", 
                    "Try again", "Add local"
                );

                if (alertResult == true)
                {
                    IsBusy = false;
                    return;
                }
            }

            IsBusy = false;

            await App.Current.MainPage.Navigation.PopToRootAsync();

            Device.BeginInvokeOnMainThread(() =>
            {
                App.Current.MainPage.DisplayAlert(
                    "Sukces!", 
                    "Miejsce zostało wysłane na serwer. Dziękujemy za dodanie nowego miejsca.", 
                    "Ok"
                );
            });
        }
    }
}
