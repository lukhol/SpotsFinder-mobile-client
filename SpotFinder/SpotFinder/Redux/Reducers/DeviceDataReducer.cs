using SpotFinder.Redux.StateModels;
using System;
using Redux;

namespace SpotFinder.Redux.Reducers
{
    public class DeviceDataReducer : IReducer<DeviceData>
    {
        public DeviceData Reduce(DeviceData localState, IAction action)
        {
            return localState;
        }
    }
}
