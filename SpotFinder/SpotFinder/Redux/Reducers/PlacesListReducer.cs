using Redux;
using SpotFinder.Models.Core;
using System.Collections.Generic;

namespace SpotFinder.Redux.Reducers
{
    public class PlacesListReducer : IReducer<List<Place>>
    {
        public List<Place> Reduce(List<Place> previousState, IAction action)
        {
            return previousState;
        }
    }
}
