using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using System;
using System.Collections.Generic;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class GetPlacesListByCriteriaCompleteAction : IAction
    {
        public Status Status { get; private set; }
        public IList<Place> PlacesList { get; private set; }
        public Exception Error { get; private set; }

        public GetPlacesListByCriteriaCompleteAction(IList<Place> placesList)
        {
            PlacesList = placesList;
            Status = Status.Success;
        }

        public GetPlacesListByCriteriaCompleteAction(Exception error)
        {
            Error = error;
            Status = Status.Error;
        }
    }
}
