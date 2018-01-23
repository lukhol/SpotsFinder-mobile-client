using Redux;
using SpotFinder.Models.Core;
using SpotFinder.Redux.StateModels;
using System.Collections.Generic;

namespace SpotFinder.Redux.Reducers
{
    public class UsersPlacesListReducer : IReducer<AsyncOperationState<IList<Place>, long>>
    {
        public AsyncOperationState<IList<Place>, long> Reduce(AsyncOperationState<IList<Place>, long> previousState, IAction action)
        {
            return previousState;
        }
    }
}
