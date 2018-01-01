using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Exceptions;
using SpotFinder.Helpers;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Redux.Actions.Locations;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.Actions.PlacesList;
using SpotFinder.Redux.StateModels;
using SpotFinder.Services;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.Config
{
    public class Bootstrapper : IBootstrapper
    {
        private IStore<ApplicationState> appStore;
        private IPermissionActionCreator permissionActionCreator;
        private IDeviceLocationActionCreator deviceLocationActionCreator;
        private IGetPlacesListByCriteriaActionCreator downloadPlacesListByCriteriaActionCreator;
        private IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator;
        private ISettingsHelper settingsHelper;
        private IErrorLogger errorLogger;

        public Bootstrapper(
            IStore<ApplicationState> appStore,
            IPermissionActionCreator permissionActionCreator,
            IDeviceLocationActionCreator deviceLocationActionCreator,
            IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator,
            IGetPlacesListByCriteriaActionCreator downloadPlacesListByCriteriaActionCreator,
            ISettingsHelper settingsHelper,
            IErrorLogger errorLogger
            )
        {
            this.appStore = appStore ?? throw new ArgumentNullException(nameof(appStore));
            this.permissionActionCreator = permissionActionCreator ?? throw new ArgumentNullException(nameof(permissionActionCreator));
            this.deviceLocationActionCreator = deviceLocationActionCreator ?? throw new ArgumentNullException(nameof(deviceLocationActionCreator));
            this.downloadPlaceByIdActionCreator = downloadPlaceByIdActionCreator ?? throw new ArgumentNullException(nameof(downloadPlaceByIdActionCreator));
            this.downloadPlacesListByCriteriaActionCreator = downloadPlacesListByCriteriaActionCreator ?? throw new ArgumentNullException(nameof(downloadPlacesListByCriteriaActionCreator));
            this.settingsHelper = settingsHelper ?? throw new ArgumentNullException(nameof(settingsHelper));
            this.errorLogger = errorLogger ?? throw new ArgumentNullException(nameof(errorLogger));
        }

        public void OnStart()
        {
            CheckPermission();
            Task.Run(() => { DeviceLocationSubscription(); });
            Task.Run(() => { DownloadInitialSpotsList(); });
            Task.Run(() => { SettingsSubscription(); });
            Task.Run(() => { ErrorSubscription(); });
        }

        public void OnSleep()
        {

        }

        public void OnResume()
        {
            CheckPermission();
        }

        private void CheckPermission()
        {
            appStore.DispatchAsync(permissionActionCreator.CheckPermissions(
                PermissionName.Location,
                PermissionName.Storage,
                PermissionName.Camera)
            );
        }

        private void DeviceLocationSubscription()
        {
            appStore
                .DistinctUntilChanged(state => new { state.DeviceData.LocationState.Status })
                .Subscribe(state =>
                {
                    AsyncOperationState<PermissionStatus, Unit> locationPermissionState = null;
                    state.PermissionsDictionary.TryGetValue(PermissionName.Location, out locationPermissionState);

                    if (locationPermissionState == null)
                    {
                        appStore.Dispatch(new SetErrorAction(
                            new LocationException("Location permission state is null."), nameof(Bootstrapper))
                        );
                        return;
                    }

                    var locationStatus = state.DeviceData.LocationState.Status;
                    if ((locationStatus == Status.Empty || locationStatus == Status.Unknown) && 
                        locationPermissionState.Value == PermissionStatus.Granted)
                        appStore.DispatchAsync(deviceLocationActionCreator.RequestDeviceLocation(TimeSpan.FromSeconds(8)));
                });

            appStore
                .DistinctUntilChanged(state => new { state.PermissionsDictionary[PermissionName.Location].Status })
                .Subscribe(state =>
                {
                    AsyncOperationState<PermissionStatus, Unit> locationPermissionState = null;
                    state.PermissionsDictionary.TryGetValue(PermissionName.Location, out locationPermissionState);

                    if (locationPermissionState == null)
                    {
                        appStore.Dispatch(new SetErrorAction(
                            new LocationException("Location permission state is null."), nameof(Bootstrapper))
                        );
                        return;
                    }

                    var locationStatus = state.DeviceData.LocationState.Status;
                    if (locationPermissionState.Value == PermissionStatus.Granted && (locationStatus == Status.Empty || locationStatus == Status.Error))
                        appStore.DispatchAsync(deviceLocationActionCreator.RequestDeviceLocation(TimeSpan.FromSeconds(8)));
                });
        }

        private void DownloadInitialSpotsList()
        {
            var city = appStore.GetState().Settings.MainCity;
            var mainDistance = appStore.GetState().Settings.MainDistance;

            if (string.IsNullOrEmpty(city))
                city = "Łódź";

            if (mainDistance < 1)
                mainDistance = 1;

            var criteria = new Criteria
            (
                new List<PlaceType> { PlaceType.Skatepark, PlaceType.Skatespot, PlaceType.DIY },
                new CityLocation
                {
                    City = city
                },
                mainDistance
            );

            appStore.DispatchAsync(downloadPlacesListByCriteriaActionCreator.DownloadPlaceByCriteria(criteria));
        }

        private void SettingsSubscription()
        {
            appStore
                .DistinctUntilChanged(state => new { state.Settings })
                .Subscribe(state => settingsHelper.SaveSettings(state.Settings));
        }

        private void ErrorSubscription()
        {
            appStore
                .DistinctUntilChanged(state => new { state.Error })
                .Subscribe(state =>
                {
                    if(state.Error != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            errorLogger.LogErrorAsync(state.Error);
                            App.Current.MainPage.DisplayAlert("Ups...", "Unexpected error occured. Information about this error will be sent to our server. We will make as much as we can to repair it!", "Ok");
                        });
                    }
                });
        }
    }
}
