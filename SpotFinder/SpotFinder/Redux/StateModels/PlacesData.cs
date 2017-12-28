using SpotFinder.Models.Core;
using System;
using System.Collections.Generic;

namespace SpotFinder.Redux.StateModels
{
    public class PlacesData
    {
        public AsyncOperationState<Place, int> CurrentPlaceState { get; private set; }
        public AsyncOperationState<IList<Place>, Criteria> PlacesListState { get; private set; }
        public Report Report { get; private set; }

        [Obsolete]
        public PlacesData()
        {

        }

        public PlacesData(
            AsyncOperationState<Place, int> currentPlaceState,
            AsyncOperationState<IList<Place>, Criteria> placesListState, 
            Report report
        )
        {
            CurrentPlaceState = currentPlaceState;
            PlacesListState = placesListState;
            Report = report;
        }
    }
}
