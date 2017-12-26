using BuilderImmutableObject;
using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.Actions.Locations;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers
{
    //TODO: In a future there should be agregate reducer (not now because only location is here)!
    public class DeviceDataReducer : IReducer<DeviceData>
    {
        public DeviceData Reduce(DeviceData previousState, IAction action)
        {
            if(action is GetDeviceLocationStartAction)
            {
                var getDeviceLocationStartAction = action as GetDeviceLocationStartAction;

                var newLocationState = previousState.LocationState
                    .Set(v => v.Status, Status.Getting)
                    .Build();

                return previousState.Set(v => v.LocationState, newLocationState)
                    .Build();
            }

            if(action is GetDeviceLocationSuccessCompleteAction)
            {
                var getDeviceLocationSuccessCompleteAction = action as GetDeviceLocationSuccessCompleteAction;

                var newLocationState = previousState.LocationState
                    .Set(v => v.Value, getDeviceLocationSuccessCompleteAction.Location)
                    .Set(v => v.Status, Status.Success)
                    .Build();

                return previousState.Set(v => v.LocationState, newLocationState)
                    .Build();
            }

            if(action is GetDeviceLocationErrorCompleteAction)
            {
                var getDeviceLocationErrorCompleteAction = action as GetDeviceLocationErrorCompleteAction;

                var newLocationState = previousState.LocationState
                    .Set(v => v.Error, getDeviceLocationErrorCompleteAction.Error)
                    .Set(v => v.Status, Status.Error)
                    .Build();

                return previousState.Set(v => v.LocationState, newLocationState)
                    .Build();
            }

            return previousState;
        }
    }
}
