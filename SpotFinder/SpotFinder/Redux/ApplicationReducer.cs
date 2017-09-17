using System;
using System.Collections.Generic;
using Redux;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux
{
    public class ApplicationReducer : IReducer<ApplicationState>
    {
        private IReducer<Stack<PageName>> NavigationReducer { get; }
        private IReducer<Settings> SettingsReducer { get; }
        private IReducer<PlacesData> PlaceDataReducer { get; }
        private IReducer<DeviceData> DeviceDataReducer { get; }

        public ApplicationReducer(IReducer<Stack<PageName>> navigationReducer, IReducer<Settings> settingsReducer,
            IReducer<PlacesData>placeDataReducer, IReducer<DeviceData> deviceDataReducer)
        {
            SettingsReducer = settingsReducer ?? throw new ArgumentNullException("SettingsReducer is null in ApplicationReducer");
            NavigationReducer = navigationReducer ?? throw new ArgumentNullException("NavigationReducer is null in ApplicationReducer");
            PlaceDataReducer = placeDataReducer ?? throw new ArgumentNullException("PlaceDataReducer is null in ApplicationReducer");
            DeviceDataReducer = deviceDataReducer ?? throw new ArgumentNullException("DeviceDataReducer is null in ApplicationReducer");
        }

        public ApplicationState Reduce(ApplicationState applicationState, IAction action)
        {
            return new ApplicationState
            {
                NavigationStack = NavigationReducer.Reduce(applicationState.NavigationStack, action),
                Settings = SettingsReducer.Reduce(applicationState.Settings, action),
                PlacesData = PlaceDataReducer.Reduce(applicationState.PlacesData, action),
                DeviceData = DeviceDataReducer.Reduce(applicationState.DeviceData, action)
            };
        }
    }
}
