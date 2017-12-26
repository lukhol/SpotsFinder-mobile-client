using Redux;
using SpotFinder.Models.Core;
using System.Collections.Generic;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class DownloadPlacesListByCriteriaCompleteAction : IAction
    {
        public IList<Place> PlacesList { get; private set; }

        public DownloadPlacesListByCriteriaCompleteAction(IList<Place> placesList)
        {
            PlacesList = placesList;
        }
    }
}
