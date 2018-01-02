using Redux;
using SpotFinder.Models.Core;
using SpotFinder.Redux.StateModels;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace SpotFinder.Redux.Reducers
{
    public class PlaceDataReducer : IReducer<PlacesData>
    {
        private IReducer<AsyncOperationState<Place, int>> currentPlaceReducer { get; }
        private IReducer<AsyncOperationState<IList<Place>, Criteria>> placeListReducer { get; }
        private IReducer<AsyncOperationState<Report, Unit>> reportReducer { get; }
        private IReducer<AsyncOperationState<WrongPlaceReport, Unit>> wrongPlaceReportReducer { get; }

        public PlaceDataReducer(
            IReducer<AsyncOperationState<Place, int>> currentPlaceReducer,
            IReducer<AsyncOperationState<IList<Place>, Criteria>> placeListReducer,
            IReducer<AsyncOperationState<Report, Unit>> reportReducer,
            IReducer<AsyncOperationState<WrongPlaceReport, Unit>> wrongPlaceReportReducer
            )
        {
            this.currentPlaceReducer = currentPlaceReducer ?? throw new ArgumentNullException(nameof(currentPlaceReducer));
            this.placeListReducer = placeListReducer ?? throw new ArgumentNullException(nameof(placeListReducer));
            this.reportReducer = reportReducer ?? throw new ArgumentNullException(nameof(reportReducer));
            this.wrongPlaceReportReducer = wrongPlaceReportReducer ?? throw new ArgumentNullException(nameof(wrongPlaceReportReducer));
        }

        public PlacesData Reduce(PlacesData previousState, IAction action)
        {
            return new PlacesData(
                currentPlaceReducer.Reduce(previousState.CurrentPlaceState, action),
                placeListReducer.Reduce(previousState.PlacesListState, action),
                reportReducer.Reduce(previousState.ReportState, action),
                wrongPlaceReportReducer.Reduce(previousState.WrongPlaceReport, action)
            );
        }
    }
}
