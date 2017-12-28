using BuilderImmutableObject;
using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.Redux.Actions.PlacesList;
using SpotFinder.Redux.StateModels;
using System.Collections.Generic;

namespace SpotFinder.Redux.Reducers
{
    public class PlacesListReducer : IReducer<AsyncOperationState<IList<Place>, Criteria>>
    {
        public AsyncOperationState<IList<Place>, Criteria> Reduce(AsyncOperationState<IList<Place>, Criteria> previousState, IAction action)
        {
            if (action is DownloadPlacesListByCriteriaStartAction)
            {
                var downloadPlacesListByCriteriaStartAction = action as DownloadPlacesListByCriteriaStartAction;

                return previousState
                    .Set(v => v.TriggerValue, downloadPlacesListByCriteriaStartAction.Criteria)
                    .Set(v => v.Status, Status.Getting)
                    .Build();
            }

            if (action is DownloadPlacesListByCriteriaCompleteAction)
            {
                var downloadPlacesListByCriteriaCompleteAction = action as DownloadPlacesListByCriteriaCompleteAction;

                return previousState
                   .Set(v => v.Value, downloadPlacesListByCriteriaCompleteAction.PlacesList)
                   .Set(v => v.Status, Status.Success)
                   .Build();
            }

            return previousState;
        }
    }
}
