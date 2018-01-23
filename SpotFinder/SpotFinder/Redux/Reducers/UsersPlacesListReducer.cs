using Redux;
using SpotFinder.Models.Core;
using SpotFinder.Redux.StateModels;
using SpotFinder.Redux.Actions.PlacesList;
using System.Collections.Generic;
using BuilderImmutableObject;

namespace SpotFinder.Redux.Reducers
{
    public class UsersPlacesListReducer : IReducer<AsyncOperationState<IList<Place>, long>>
    {
        public AsyncOperationState<IList<Place>, long> Reduce(AsyncOperationState<IList<Place>, long> previousState, IAction action)
        {
            if(action is GetPlacesStartAction<long>)
            {
                var getPlacesListByUserIdAction = action as GetPlacesStartAction<long>;

                return previousState
                    .Set(v => v.TriggerValue, getPlacesListByUserIdAction.Value)
                    .Set(v => v.Status, getPlacesListByUserIdAction.Status)
                    .Build();
            }

            if(action is GetPlacesListByUserIdCompleteAction)
            {
                var getPlacesListByUserIdCompleteAction = action as GetPlacesListByUserIdCompleteAction;

                return previousState
                   .Set(v => v.Value, getPlacesListByUserIdCompleteAction.PlacesList)
                   .Set(v => v.Status, getPlacesListByUserIdCompleteAction.Status)
                   .Set(v => v.Error, getPlacesListByUserIdCompleteAction.Error)
                   .Build();
            }

            return previousState;
        }
    }
}
