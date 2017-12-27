using SpotFinder.Redux.StateModels;
using System;
using Redux;
using SpotFinder.Redux.Actions;
using System.Collections.Generic;
using SpotFinder.Models.Core;
using SpotFinder.Services;
using BuilderImmutableObject;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.Actions.PlacesList;

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
                    .Set(v => v.Criteria, downloadPlacesListByCriteriaStartAction.Criteria)
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
                var report = new Report(new Place(), new Location(0 , 0));
                previousState.Set(v => v.Report, report)
                    .Build();
            }

            if (action is ClearReportAction)
            {
                return previousState.Set(v => v.Report, null)
                    .Build();
            }

            if (action is PassPlaceToReportAction)
            {
                var passPlaceToReportAction = action as PassPlaceToReportAction;

                var newReportObject = previousState.Report
                    .Set(v => v.Place, passPlaceToReportAction.Place)
                    .Build();

                return previousState.Set(v => v.Report, newReportObject)
                    .Build();
            }

            if (action is UpdateLocationInReportAction)
            {
                //TODO: Place HAVE TO be immutable.
                var updateLocationInReportAction = action as UpdateLocationInReportAction;

                previousState.Report.Place.Location = updateLocationInReportAction.Location;

                var newReportState = previousState.Report
                    .Set(v => v.Place, previousState.Report.Place)
                    .Set(v => v.Location, updateLocationInReportAction.Location)
                    .Build();

                return previousState.Set(v => v.Report, newReportState)
                    .Build();
            }

            //Downloading single spot:
            if (action is DownloadPlaceByIdStartAction)
            {
                var downloadPlaceByIdStartAction = action as DownloadPlaceByIdStartAction;

                var newCurrentPlaceState = previousState.CurrentPlaceState
                    .Set(v => v.Id, downloadPlaceByIdStartAction.Id)
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
