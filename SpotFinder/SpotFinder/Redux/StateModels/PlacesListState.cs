using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using System;
using System.Collections.Generic;

namespace SpotFinder.Redux.StateModels
{
    public class PlacesListState
    {
        public Criteria Criteria { get; private set; }
        public Status Status { get; private set; }
        public Exception Error { get; private set; }
        public IList<Place> Value { get; private set; }

        [Obsolete]
        public PlacesListState()
        {

        }

        public PlacesListState(Criteria criteria, Status status, Exception error, IList<Place> value)
        {
            Criteria = criteria;
            Status = status;
            Error = error;
            Value = value;
        }
    }
}
