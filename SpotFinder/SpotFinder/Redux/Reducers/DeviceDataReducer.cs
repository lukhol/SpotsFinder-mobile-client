using SpotFinder.Redux.StateModels;
using System;
using Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Models.Core;
using SpotFinder.Services;

namespace SpotFinder.Redux.Reducers
{
    public class DeviceDataReducer : IReducer<DeviceData>
    {
        private IDeviceLocationProvider DeviceLocationProvider { get; }

        public DeviceDataReducer(IDeviceLocationProvider deviceLocationProvider)
        {
            DeviceLocationProvider = deviceLocationProvider ?? throw new ArgumentNullException("DeviceLocationProvider is null in DeviceDataReducer");
        }

        public DeviceData Reduce(DeviceData previousState, IAction action)
        {
            if(action is UpdateDeviceLocationAction)
            {
                var updateDeviceLocationAction = action as UpdateDeviceLocationAction;

                var location = new Location
                {
                    Latitude = updateDeviceLocationAction.Latitude,
                    Longitude = updateDeviceLocationAction.Longitude
                };

                previousState.Location = location;

                return previousState;
            }

            if(action is RequestGettingDeviceLocationOnce)
            {
                if (DeviceLocationProvider.IsAcquiring == false)
                    DeviceLocationProvider.RequestDeviceLocationOnesAsync();

                return previousState;
            }

            if(action is RequestGettingDeviceLocationLoop)
            {
                if (DeviceLocationProvider.IsLoopActivated == false)
                    DeviceLocationProvider.RequestDeviceLocationLoopAsync();

                return previousState;
            }

            if(action is ClearDeviceLocationAction)
            {
                previousState.Location = null;
                return previousState;
            }

            return previousState;
        }
    }
}
