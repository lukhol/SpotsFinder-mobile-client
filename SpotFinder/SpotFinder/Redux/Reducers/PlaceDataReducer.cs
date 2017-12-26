using SpotFinder.Redux.StateModels;
using System;
using Redux;
using SpotFinder.Redux.Actions;
using System.Collections.Generic;
using SpotFinder.Models.Core;
using SpotFinder.Services;

namespace SpotFinder.Redux.Reducers
{
    public class PlaceDataReducer : IReducer<PlacesData>
    {
        private IPlaceManager PlaceManager { get; }

        public PlaceDataReducer(IPlaceManager placeManager)
        {
            PlaceManager = placeManager ?? throw new ArgumentNullException("PlaceManager is null in PlaceDataReducer");
        }

        public PlacesData Reduce(PlacesData previousState, IAction action)
        {
            //List of spots:
            if(action is ListOfPlacesAction)
            {
                var listOfPlacesAction = action as ListOfPlacesAction;

                var newListOfPlaces = new List<Place>();

                if (listOfPlacesAction.ListOfPlaces == null)
                    return previousState;

                foreach(var tempPlace in listOfPlacesAction.ListOfPlaces)
                {
                    newListOfPlaces.Add(tempPlace);
                }

                previousState.ListOfPlaces = newListOfPlaces;

                return previousState;
            }

            if(action is ReplaceCriteriaAction)
            {
                var replaceCriteriaAction = action as ReplaceCriteriaAction;

               previousState.Criteria = replaceCriteriaAction.Criteria;

                return previousState;
            }

            if(action is SetInitialCriteriaAction)
            {
                var setInitialCriteriaAction = action as SetInitialCriteriaAction;

                previousState.Criteria = setInitialCriteriaAction.Criteria;

                return previousState;
            }

            if(action is ClearSpotsListAction)
            {
                previousState.ListOfPlaces = null;

                return previousState;
            }

            //Adding new place:
            if(action is CreateNewReportAction)
            {
                previousState.Report = new Report();
                return previousState;
            }

            if(action is ClearReportAction)
            {
                previousState.Report = null;
                return previousState;
            }

            if(action is PassPlaceToReportAction)
            {
                var passPlaceToReportAction = action as PassPlaceToReportAction;
                previousState.Report.Place = passPlaceToReportAction.Place;

                return previousState;
            }

            if(action is PassLocationToReportingPlaceAction)
            {
                var passLocationToReportingPlaceAction = action as PassLocationToReportingPlaceAction;
                if(passLocationToReportingPlaceAction.Latitude == null)
                {
                    previousState.Report.Place.Location = new Location
                    {
                        Longitude = previousState.Report.Location.Longitude,
                        Latitude = previousState.Report.Location.Latitude
                    };
                }
                else
                {
                    previousState.Report.Place.Location = new Location
                    {
                        Longitude = (double)passLocationToReportingPlaceAction.Longitude,
                        Latitude = (double)passLocationToReportingPlaceAction.Latitude
                    };
                }

                return previousState;
            }

            if(action is UpdateLocationInReportAction)
            {
                var updateLocationInReportAction = action as UpdateLocationInReportAction;
                var location = new Location
                {
                    Longitude = updateLocationInReportAction.Longitude,
                    Latitude = updateLocationInReportAction.Latitude
                };

                if(previousState.Report != null)
                    previousState.Report.Location = location;

                return previousState;
            }
 
            //Downloading single spot:
            if(action is ClearActuallyShowingPlaceAction)
            {
                previousState.ShowingPlace = null;
                return previousState;
            }

            if(action is RequestDownloadSpotAction)
            {
                var requestDownloadSpotAction = action as RequestDownloadSpotAction;
                //This probably should be in App.xaml.cs in subscription.
                PlaceManager.DownloadSinglePlaceByIdAsync(requestDownloadSpotAction.Id);
                return previousState;
            }

            if(action is PassSpotToShowAction)
            {
                var passSpotToShowAction = action as PassSpotToShowAction;
                previousState.ShowingPlace = passSpotToShowAction.Place;
            }

            return previousState;
        }
    }
}
