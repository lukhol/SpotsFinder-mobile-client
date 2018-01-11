using Redux;
using SpotFinder.Redux.StateModels;
using System.Reactive;

namespace SpotFinder.Redux.Reducers.Users
{
    public class RegisterReducer : IReducer<AsyncOperationState<User, Unit>>
    {
        public AsyncOperationState<User, Unit> Reduce(AsyncOperationState<User, Unit> previousState, IAction action)
        {
            return previousState;
        }
    }
}
