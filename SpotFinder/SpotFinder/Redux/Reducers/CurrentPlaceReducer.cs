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
            if (action is DownloadPlaceByIdStartAction)
            {
                var downloadPlaceByIdStartAction = action as DownloadPlaceByIdStartAction;

                return previousState
                    .Set(v => v.TriggerValue, downloadPlaceByIdStartAction.Id)
                    .Set(v => v.Status, Status.Getting)
                    .Build();
            }

            if (action is DownloadPlaceByIdCompleteAction)
            {
                var downloadPlaceByIdCompleteAction = action as DownloadPlaceByIdCompleteAction;

                return previousState
                    .Set(v => v.Value, downloadPlaceByIdCompleteAction.Place)
                    .Set(v => v.Status, Status.Success)
                    .Build();
            }

            return previousState;
        }
    }
}
