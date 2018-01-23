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
            if (action is GetPlacesStartAction<Criteria>)
            {
                var downloadPlacesListByCriteriaStartAction = action as GetPlacesStartAction<Criteria>;

                return previousState
                    .Set(v => v.TriggerValue, downloadPlacesListByCriteriaStartAction.Value)
                    .Set(v => v.Status, downloadPlacesListByCriteriaStartAction.Status)
                    .Build();
            }

            if (action is GetPlacesListByCriteriaCompleteAction)
            {
                var downloadPlacesListByCriteriaCompleteAction = action as GetPlacesListByCriteriaCompleteAction;

                return previousState
                   .Set(v => v.Value, downloadPlacesListByCriteriaCompleteAction.PlacesList)
                   .Set(v => v.Status, downloadPlacesListByCriteriaCompleteAction.Status)
                   .Set(v => v.Error, downloadPlacesListByCriteriaCompleteAction.Error)
                   .Build();
            }

            return previousState;
        }
    }
}
