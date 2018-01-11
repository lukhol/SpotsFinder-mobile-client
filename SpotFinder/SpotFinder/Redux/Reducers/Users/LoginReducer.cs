using BuilderImmutableObject;
using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Redux.StateModels;
using System.Reactive;

namespace SpotFinder.Redux.Reducers.Users
{
    public class LoginReducer : IReducer<AsyncOperationState<User, AccessProvider>>
    {
        public AsyncOperationState<User, AccessProvider> Reduce(AsyncOperationState<User, AccessProvider> previousState, IAction action)
        {
            if(action is SetLoginStartAction)
            {
                var setLoginStartAction = action as SetLoginStartAction;

                return previousState
                    .Set(v => v.Value, setLoginStartAction.User)
                    .Set(v => v.Status, setLoginStartAction.Status)
                    .Set(v => v.Error, setLoginStartAction.Error)
                    .Build();
            }

            if(action is SetLoginCompleteAction)
            {
                var setLoginCompleteAction = action as SetLoginCompleteAction;

                return previousState
                    .Set(v => v.Error, setLoginCompleteAction.Error)
                    .Set(v => v.Status, setLoginCompleteAction.Status)
                    .Set(v => v.Value, setLoginCompleteAction.User)
                    .Build();
            }

            return previousState;
        }
    }
}
