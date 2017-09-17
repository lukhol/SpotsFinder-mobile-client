using SpotFinder.Redux.StateModels;
using System;
using Redux;

namespace SpotFinder.Redux.Reducers
{
    public class PlaceDataReducer : IReducer<PlacesData>
    {
        public PlacesData Reduce(PlacesData localState, IAction action)
        {
            return localState;
        }
    }
}
