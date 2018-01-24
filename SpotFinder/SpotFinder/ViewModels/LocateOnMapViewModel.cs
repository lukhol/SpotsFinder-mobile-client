using Redux;
using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.OwnControls;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Repositories;
using SpotFinder.Resx;
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

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
                .SubscribeWithError(state =>
                {
                    if (state.PlacesData.ReportState.Value.ReportType == Core.Enums.ReportType.Create)
                    {
                        var stateLocation = state.DeviceData.LocationState.Value;
                        mapCenterLocation = new Position(stateLocation.Latitude, stateLocation.Longitude);
                    }
                    else
                    {
                        var updatingPlaceLocation = state.PlacesData.ReportState.Value.Location;
                        mapCenterLocation = new Position(updatingPlaceLocation.Latitude, updatingPlaceLocation.Longitude);
                    }
                }, error => { appStore.Dispatch(new SetErrorAction(error, "LocateOnMapViewModel - subscription.")); });

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

            if(map.VisibleRegion != null)
                appStore.Dispatch(new SetReportLocationAction(
                    new Location(map.VisibleRegion.Center.Latitude, map.VisibleRegion.Center.Longitude)
                    )
                );

            var user = appStore.GetState().UserState.User;
            if (user != null)
                appStore.Dispatch(new SetReportUserAction(user));

            var addingPlace = appStore.GetState().PlacesData.ReportState.Value.Place;

            long result = 0;

            if(addingPlace.Id == 0)
            {
                result = await PlaceService.SendAsync(addingPlace);
            }
            else
            {
                try
                {
                    result = await PlaceService.UpdateAsync(addingPlace, addingPlace.Id);
                }
                catch (Exception e)
                {

                }
            }

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
