using System;

namespace SpotFinder.Redux.StateModels
{
    public class DeviceData
    {
        public LocationState LocationState { get; private set; }

        [Obsolete]
        public DeviceData()
        {
        }

        public DeviceData(LocationState locationState)
        {
            LocationState = locationState;
        }
    }
}
