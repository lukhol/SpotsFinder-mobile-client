using BuilderImmutableObject;
using Redux;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers.Users
{
    public class EditReducer : IReducer<AsyncOperationState<User, object>>
    {
        public AsyncOperationState<User, object> Reduce(AsyncOperationState<User, object> previousState, IAction action)
        {
            if(action is SetUpdateUserStartAction)
            {
                var setUpdateUserStartAction = action as SetUpdateUserStartAction;

                return previousState
                    .Set(v => v.Value, setUpdateUserStartAction.User)
                    .Set(v => v.Status, setUpdateUserStartAction.Status)
                    .Set(v => v.Error, setUpdateUserStartAction.Error)
                    .Build();
            }

            if (action is SetUpdateUserCompleteAction)
            {
                var setUpdateUsercompleteAction = action as SetUpdateUserCompleteAction;

                return previousState
                    .Set(v => v.Value, setUpdateUsercompleteAction.User)
                    .Set(v => v.Status, setUpdateUsercompleteAction.Status)
                    .Set(v => v.Error, setUpdateUsercompleteAction.Error)
                    .Build();
            }

            return previousState;
        }
    }
}
