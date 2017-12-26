using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.StateModels
{
    public class CurrentPlaceState
    {
        public int Id { get; private set; }
        public Status Status { get; private set; }
        public Exception Error { get; private set; }
        public Place Value { get; private set; }

        [Obsolete]
        public CurrentPlaceState()
        {

        }

        public CurrentPlaceState(int id, Status status, Exception error, Place value)
        {
            Id = id;
            Status = status;
            Error = error;
            Value = value;
        }
    }
}
