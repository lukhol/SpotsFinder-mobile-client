using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotFinder.Models.Core;
using Redux;

namespace SpotFinder.Redux.Actions
{
    public class ListOfPlacesAction : IAction
    {
        public List<Place> ListOfPlaces { get; private set; }

        public ListOfPlacesAction(List<Place> listOfPlaces)
        {
            ListOfPlaces = listOfPlaces;
        }
    }
}
