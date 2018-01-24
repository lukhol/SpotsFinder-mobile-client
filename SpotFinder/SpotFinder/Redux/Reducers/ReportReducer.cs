using BuilderImmutableObject;
using Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.StateModels;
using System.Reactive;

namespace SpotFinder.Redux.Reducers
{
    public class ReportReducer : IReducer<AsyncOperationState<Report, Unit>>
    {
        public AsyncOperationState<Report, Unit> Reduce(AsyncOperationState<Report, Unit> previousState, IAction action)
        {
            if (action is SetNewReportAction)
            {
                return previousState
                    .Set(v => v.Value, new Report(null, null, Core.Enums.ReportType.Create))
                    .Build();
            }

            if(action is SetUpdateReportPlaceAction)
            {
                var setUpdateReportPlaceAction = action as SetUpdateReportPlaceAction;

                return previousState
                    .Set(v => v.Value, new Report(setUpdateReportPlaceAction.Place, setUpdateReportPlaceAction.Place.Location, Core.Enums.ReportType.Update))
                    .Build();
            }

            if (action is SetReportPlaceAction)
            {
                var passPlaceToReportAction = action as SetReportPlaceAction;

                var newReportObject = previousState.Value
                    .Set(v => v.Place, passPlaceToReportAction.Place)
                    .Build();

                return previousState
                    .Set(v => v.Value, newReportObject)
                    .Build();
            }

            if(action is SetReportUserAction)
            {
                var setReportUserAction = action as SetReportUserAction;

                var updatedPlace = previousState.Value.Place;
                updatedPlace.UserId = setReportUserAction.User.Id;

                var newReportObject = previousState.Value
                    .Set(v => v.Place, updatedPlace)
                    .Build();

                return previousState
                    .Set(v => v.Value, newReportObject)
                    .Build();
            }

            if (action is SetReportLocationAction)
            {
                //TODO: Place HAVE TO be immutable.
                var updateLocationInReportAction = action as SetReportLocationAction;

                previousState.Value.Place.Location = updateLocationInReportAction.Location;

                var newReportObject = previousState.Value
                    .Set(v => v.Location, updateLocationInReportAction.Location)
                    .Build();

                return previousState
                    .Set(v => v.Value, newReportObject)
                    .Build();
            }

            return previousState;
        }
    }
}
