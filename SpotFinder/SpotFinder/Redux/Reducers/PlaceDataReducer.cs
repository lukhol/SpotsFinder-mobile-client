using BuilderImmutableObject;
using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Redux.Actions.PlacesList;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers
{
    public class PlaceDataReducer : IReducer<PlacesData>
    {
        public PlacesData Reduce(PlacesData previousState, IAction action)
        {
            //List of spots:
            if(action is DownloadPlacesListByCriteriaStartAction)
            {
                var downloadPlacesListByCriteriaStartAction = action as DownloadPlacesListByCriteriaStartAction;

                var newPlacesListState = previousState.PlacesListState
                    .Set(v => v.TriggerValue, downloadPlacesListByCriteriaStartAction.Criteria)
                    .Set(v => v.Status, Status.Getting)
                    .Build();

                return previousState.Set(v => v.PlacesListState, newPlacesListState)
                    .Build();
            }

            if(action is DownloadPlacesListByCriteriaCompleteAction)
            {
                var downloadPlacesListByCriteriaCompleteAction = action as DownloadPlacesListByCriteriaCompleteAction;

                var newPlacesListState = previousState.PlacesListState
                   .Set(v => v.Value, downloadPlacesListByCriteriaCompleteAction.PlacesList)
                   .Set(v => v.Status, Status.Success)
                   .Build();

                return previousState.Set(v => v.PlacesListState, newPlacesListState)
                    .Build();
            }

            //Adding new place:
            if (action is CreateNewReportAction)
            {
                var newReportState = previousState.ReportState
                    .Set(v => v.Value, new Report(null, null))
                    .Build();

                previousState.Set(v => v.ReportState, newReportState)
                    .Build();
            }

            if (action is PassPlaceToReportAction)
            {
                var passPlaceToReportAction = action as PassPlaceToReportAction;

                var newReportObject = previousState.ReportState.Value
                    .Set(v => v.Place, passPlaceToReportAction.Place)
                    .Build();

                var newReportState = previousState.ReportState
                    .Set(v => v.Value, newReportObject)
                    .Build();

                return previousState.Set(v => v.ReportState, newReportState)
                    .Build();
            }

            if (action is UpdateLocationInReportAction)
            {
                //TODO: Place HAVE TO be immutable.
                var updateLocationInReportAction = action as UpdateLocationInReportAction;

                previousState.ReportState.Value.Place.Location = updateLocationInReportAction.Location;

                var newReportObject = previousState.ReportState.Value
                    .Set(v => v.Location, updateLocationInReportAction.Location)
                    .Build();

                var newReportState = previousState.ReportState
                    .Set(v => v.Value, newReportObject)
                    .Build();

                return previousState.Set(v => v.ReportState, newReportState)
                    .Build();
            }

            //Downloading single spot:
            if (action is DownloadPlaceByIdStartAction)
            {
                var downloadPlaceByIdStartAction = action as DownloadPlaceByIdStartAction;

                var newCurrentPlaceState = previousState.CurrentPlaceState
                    .Set(v => v.TriggerValue, downloadPlaceByIdStartAction.Id)
                    .Set(v => v.Status, Status.Getting)
                    .Build();
                
                return previousState
                    .Set(v => v.CurrentPlaceState, newCurrentPlaceState)
                    .Build();
            }

            if (action is DownloadPlaceByIdCompleteAction)
            {
                var downloadPlaceByIdCompleteAction = action as DownloadPlaceByIdCompleteAction;
                var newCurrentPlaceState = previousState.CurrentPlaceState
                    .Set(v => v.Value, downloadPlaceByIdCompleteAction.Place)
                    .Set(v => v.Status, Status.Success)
                    .Build();

                return previousState.Set(v => v.CurrentPlaceState, newCurrentPlaceState)
                    .Build();
            }

            return previousState;
        }
    }
}
