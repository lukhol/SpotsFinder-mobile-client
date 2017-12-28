using SpotFinder.Models.Core;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace SpotFinder.Redux.StateModels
{
    public class PlacesData
    {
        public AsyncOperationState<Place, int> CurrentPlaceState { get; private set; }
        public AsyncOperationState<IList<Place>, Criteria> PlacesListState { get; private set; }
        public AsyncOperationState<Report, Unit> ReportState { get; private set; }

        [Obsolete]
        public PlacesData()
        {

        }

        public PlacesData(
            AsyncOperationState<Place, int> currentPlaceState,
            AsyncOperationState<IList<Place>, Criteria> placesListState,
            AsyncOperationState<Report, Unit> reportState
        )
        {
            CurrentPlaceState = currentPlaceState;
            PlacesListState = placesListState;
            ReportState = reportState;
        }
    }
}
