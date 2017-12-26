using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions.Locations;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.StateModels;
using System;
using System.Reactive.Linq;

namespace SpotFinder.Config
{
    public class Bootstrapper
    {
        private IStore<ApplicationState> appStore;
        private IPermissionActionCreator permissionActionCreator;
        private IDeviceLocationActionCreator deviceLocationActionCreator;


        public Bootstrapper(
            IStore<ApplicationState> appStore,
            IPermissionActionCreator permissionActionCreator,
            IDeviceLocationActionCreator deviceLocationActionCreator
            )
        {
            this.appStore = appStore ?? throw new ArgumentNullException(nameof(appStore));
            this.permissionActionCreator = permissionActionCreator ?? throw new ArgumentNullException(nameof(permissionActionCreator));
            this.deviceLocationActionCreator = deviceLocationActionCreator ?? throw new ArgumentNullException(nameof(deviceLocationActionCreator));
        }

        public void OnStart()
        {
            CheckPermission();
            DeviceLocationSubscription();
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
                    if ((locationStatus == Status.NotStartedYet || locationStatus == Status.Unknown) && 
                        locationPermission.PermissionStatus == PermissionStatus.Granted)
                        appStore.DispatchAsync(deviceLocationActionCreator.RequestDeviceLocation(TimeSpan.FromSeconds(8)));
                });
        }
    }
}
