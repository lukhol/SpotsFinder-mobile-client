using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Reducers
{
    public class CurrentPlaceReducer : IReducer<Place>
    {
        public Place Reduce(Place previousState, IAction action)
        {
            return previousState;
        }
    }
}
