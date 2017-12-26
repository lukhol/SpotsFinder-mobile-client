using Redux;
using SpotFinder.Models.Core;
using System.Collections.Generic;

namespace SpotFinder.Redux.Actions
{
    public class SetListOfPlacesAction : IAction
    {
        public List<Place> ListOfPlaces { get; private set; }

        public SetListOfPlacesAction(List<Place> listOfPlaces)
        {
            ListOfPlaces = listOfPlaces;
        }
    }
}
