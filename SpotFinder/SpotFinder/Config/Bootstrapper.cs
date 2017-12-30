using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Helpers;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Redux.Actions.Locations;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.Actions.PlacesList;
using SpotFinder.Redux.StateModels;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

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

        public Bootstrapper(
            IStore<ApplicationState> appStore,
            IPermissionActionCreator permissionActionCreator,
            IDeviceLocationActionCreator deviceLocationActionCreator,
            IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator,
            IGetPlacesListByCriteriaActionCreator downloadPlacesListByCriteriaActionCreator,
            ISettingsHelper settingsHelper
            )
        {
            this.appStore = appStore ?? throw new ArgumentNullException(nameof(appStore));
            this.permissionActionCreator = permissionActionCreator ?? throw new ArgumentNullException(nameof(permissionActionCreator));
            this.deviceLocationActionCreator = deviceLocationActionCreator ?? throw new ArgumentNullException(nameof(deviceLocationActionCreator));
            this.downloadPlaceByIdActionCreator = downloadPlaceByIdActionCreator ?? throw new ArgumentNullException(nameof(downloadPlaceByIdActionCreator));
            this.downloadPlacesListByCriteriaActionCreator = downloadPlacesListByCriteriaActionCreator ?? throw new ArgumentNullException(nameof(downloadPlacesListByCriteriaActionCreator));
            this.settingsHelper = settingsHelper ?? throw new ArgumentNullException(nameof(settingsHelper));
        }

        public void OnStart()
        {
            CheckPermission();
            Task.Run(() => { DeviceLocationSubscription(); });
            Task.Run(() => { DownloadInitialSpotsList(); });
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
                        //TODO: Error message!!
                        return;
                    }

                    var locationStatus = state.DeviceData.LocationState.Status;
                    if ((locationStatus == Status.Empty || locationStatus == Status.Unknown) && 
                        locationPermissionState.Value == PermissionStatus.Granted)
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
    }
}
