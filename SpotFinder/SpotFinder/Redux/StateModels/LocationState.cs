using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.StateModels
{
    public class LocationState
    {
        public Status Status { get; private set; }
        public Exception Error { get; private set; }
        public Location Value { get; private set; }

        [Obsolete]
        public LocationState()
        {

        }

        public LocationState(Status status, Exception error, Location value)
        {
            Status = status;
            Error = error;
            Value = value;
        }
    }
}
