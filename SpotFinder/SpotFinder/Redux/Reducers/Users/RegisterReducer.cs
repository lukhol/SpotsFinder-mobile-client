using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers.Users
{
    public class RegisterReducer : IReducer<AsyncOperationState<User, AccessProvider>>
    {
        public AsyncOperationState<User, AccessProvider> Reduce(AsyncOperationState<User, AccessProvider> previousState, IAction action)
        {
            return previousState;
        }
    }
}
