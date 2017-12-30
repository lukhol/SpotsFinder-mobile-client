using BuilderImmutableObject;
using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers
{
    public class CurrentPlaceReducer : IReducer<AsyncOperationState<Place, int>>
    {
        public AsyncOperationState<Place, int> Reduce(AsyncOperationState<Place, int> previousState, IAction action)
        {
            if (action is GetPlaceByIdStartAction)
            {
                var downloadPlaceByIdStartAction = action as GetPlaceByIdStartAction;

                return previousState
                    .Set(v => v.TriggerValue, downloadPlaceByIdStartAction.Id)
                    .Set(v => v.Status, Status.Getting)
                    .Build();
            }

            if (action is GetPlaceByIdCompleteAction)
            {
                var downloadPlaceByIdCompleteAction = action as GetPlaceByIdCompleteAction;

                return previousState
                    .Set(v => v.Value, downloadPlaceByIdCompleteAction.Place)
                    .Set(v => v.Status, downloadPlaceByIdCompleteAction.Status)
                    .Set(v => v.Error, downloadPlaceByIdCompleteAction.Error)
                    .Build();
            }

            return previousState;
        }
    }
}
