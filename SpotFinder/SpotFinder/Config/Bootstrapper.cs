using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Redux.Actions.Locations;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.Actions.PlacesList;
using SpotFinder.Redux.StateModels;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace SpotFinder.Config
{
    public class Bootstrapper : IBootstrapper
    {
        private IStore<ApplicationState> appStore;
        private IPermissionActionCreator permissionActionCreator;
        private IDeviceLocationActionCreator deviceLocationActionCreator;
        private IDownloadPlacesListByCriteriaActionCreator downloadPlacesListByCriteriaActionCreator;
        private IDownloadPlaceByIdActionCreator downloadPlaceByIdActionCreator;

        public Bootstrapper(
            IStore<ApplicationState> appStore,
            IPermissionActionCreator permissionActionCreator,
            IDeviceLocationActionCreator deviceLocationActionCreator,
            IDownloadPlaceByIdActionCreator downloadPlaceByIdActionCreator,
            IDownloadPlacesListByCriteriaActionCreator downloadPlacesListByCriteriaActionCreator
            )
        {
            this.appStore = appStore ?? throw new ArgumentNullException(nameof(appStore));
            this.permissionActionCreator = permissionActionCreator ?? throw new ArgumentNullException(nameof(permissionActionCreator));
            this.deviceLocationActionCreator = deviceLocationActionCreator ?? throw new ArgumentNullException(nameof(deviceLocationActionCreator));
            this.downloadPlaceByIdActionCreator = downloadPlaceByIdActionCreator ?? throw new ArgumentNullException(nameof(downloadPlaceByIdActionCreator));
            this.downloadPlacesListByCriteriaActionCreator = downloadPlacesListByCriteriaActionCreator ?? throw new ArgumentNullException(nameof(downloadPlacesListByCriteriaActionCreator));
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
                    Permission locationPermission = null;
                    state.PermissionsDictionary.TryGetValue(PermissionName.Location, out locationPermission);

                    if (locationPermission == null)
                    {
                        //TODO: Error message!!
                        return;
                    }

                    var locationStatus = state.DeviceData.LocationState.Status;
                    if ((locationStatus == Status.Empty || locationStatus == Status.Unknown) && 
                        locationPermission.PermissionStatus == PermissionStatus.Granted)
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
            {
                Location = new CityLocation
                {
                    City = city
                },
                Distance = mainDistance,
                Type = new List<PlaceType> { PlaceType.Skatepark, PlaceType.Skatespot, PlaceType.DIY }
            };

            appStore.DispatchAsync(downloadPlacesListByCriteriaActionCreator.DownloadPlaceByCriteria(criteria));
        }
    }
}
