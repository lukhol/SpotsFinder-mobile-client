using BuilderImmutableObject;
using Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers
{
    public class ErrorReducer : IReducer<ErrorState>
    {
        public ErrorState Reduce(ErrorState previousState, IAction action)
        {
            if(action is SetErrorAction)
            {
                var setErrorAction = action as SetErrorAction;

                return previousState
                    .Set(v => v.Error, setErrorAction.Error)
                    .Set(v => v.WhereOccurred, setErrorAction.WhereOccurred)
                    .Build();
            }

            return previousState;
        }
    }
}
