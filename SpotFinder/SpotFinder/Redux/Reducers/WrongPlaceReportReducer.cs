using BuilderImmutableObject;
using Redux;
using SpotFinder.Redux.Actions.WrongPlaceReports;
using SpotFinder.Redux.StateModels;
using System.Reactive;

namespace SpotFinder.Redux.Reducers
{
    public class WrongPlaceReportReducer : IReducer<AsyncOperationState<WrongPlaceReport, Unit>>
    {
        public AsyncOperationState<WrongPlaceReport, Unit> Reduce(AsyncOperationState<WrongPlaceReport, Unit> previousState, IAction action)
        {
            if (action is SetWrongPlaceReportStartAction)
            {
                var setWrongPlaceReportStartAction = action as SetWrongPlaceReportStartAction;

                return previousState
                    .Set(v => v.Status, setWrongPlaceReportStartAction.Status)
                    .Set(v => v.Value, setWrongPlaceReportStartAction.WrongPlaceReport)
                    .Set(v => v.Error, null)
                    .Build();
            }

            if(action is SetWrongPlaceReportCompleteAction)
            {
                var setWrongPlaceReportCompleteAction = action as SetWrongPlaceReportCompleteAction;

                return previousState
                    .Set(v => v.Status, setWrongPlaceReportCompleteAction.Status)
                    .Set(v => v.Value, setWrongPlaceReportCompleteAction.WrongPlaceReport)
                    .Set(v => v.Error, setWrongPlaceReportCompleteAction.Error)
                    .Build();
            }

            if(action is SetEmptyWrongPlaceReportAction)
            {
                return previousState
                    .Set(v => v.Status, Core.Enums.Status.Empty)
                    .Set(v => v.Value, null)
                    .Set(v => v.Error, null)
                    .Build();
            }

            return previousState;
        }
    }
}
