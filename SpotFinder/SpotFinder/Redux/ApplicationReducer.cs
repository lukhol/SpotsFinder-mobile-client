using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;
using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux
{
    public class ApplicationReducer : IReducer<ApplicationState>
    {
        private IReducer<IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>>> PermissionsReducer { get; }
        private IReducer<Stack<PageName>> NavigationReducer { get; }
        private IReducer<Settings> SettingsReducer { get; }
        private IReducer<PlacesData> PlaceDataReducer { get; }
        private IReducer<DeviceData> DeviceDataReducer { get; }
        private IReducer<ErrorState> ErrorReducer { get; }

        public ApplicationReducer(
            IReducer<IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>>> permissionsReducer,
            IReducer<Stack<PageName>> navigationReducer,
            IReducer<Settings> settingsReducer,
            IReducer<PlacesData>placeDataReducer, 
            IReducer<DeviceData> deviceDataReducer,
            IReducer<ErrorState> errorReducer
        )
        {
            PermissionsReducer = permissionsReducer ?? throw new ArgumentNullException(nameof(PermissionsReducer));
            SettingsReducer = settingsReducer ?? throw new ArgumentNullException("SettingsReducer is null in ApplicationReducer");
            NavigationReducer = navigationReducer ?? throw new ArgumentNullException("NavigationReducer is null in ApplicationReducer");
            PlaceDataReducer = placeDataReducer ?? throw new ArgumentNullException("PlaceDataReducer is null in ApplicationReducer");
            DeviceDataReducer = deviceDataReducer ?? throw new ArgumentNullException("DeviceDataReducer is null in ApplicationReducer");
            ErrorReducer = errorReducer ?? throw new ArgumentNullException(nameof(errorReducer));
        }

        public ApplicationState Reduce(ApplicationState applicationState, IAction action)
        {
            return new ApplicationState(
                PermissionsReducer.Reduce(applicationState.PermissionsDictionary, action),
                NavigationReducer.Reduce(applicationState.NavigationStack, action),
                SettingsReducer.Reduce(applicationState.Settings, action),
                PlaceDataReducer.Reduce(applicationState.PlacesData, action),
                DeviceDataReducer.Reduce(applicationState.DeviceData, action),
                ErrorReducer.Reduce(applicationState.Error, action)
            );
        }
    }
}
