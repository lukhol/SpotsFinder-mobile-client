using BuilderImmutableObject;
using Redux;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Redux.StateModels;
using System.Reactive;

namespace SpotFinder.Redux.Reducers.Users
{
    public class RegisterReducer : IReducer<AsyncOperationState<User, Unit>>
    {
        public AsyncOperationState<User, Unit> Reduce(AsyncOperationState<User, Unit> previousState, IAction action)
        {
            if(action is SetRegisterUserStartAction)
            {
                var setRegisterUserStartAction = action as SetRegisterUserStartAction;

                return previousState
                    .Set(v => v.Error, setRegisterUserStartAction.Error)
                    .Set(v => v.Status, setRegisterUserStartAction.Status)
                    .Set(v => v.Value, setRegisterUserStartAction.User)
                    .Build();
            }

            if(action is SetRegisterUserCompleteAction)
            {
                var setRegisterCompleteAction = action as SetRegisterUserCompleteAction;

                return previousState
                    .Set(v => v.Error, setRegisterCompleteAction.Error)
                    .Set(v => v.Status, setRegisterCompleteAction.Status)
                    .Set(v => v.Value, setRegisterCompleteAction.User)
                    .Build();
            }

            return previousState;
        }
    }
}
