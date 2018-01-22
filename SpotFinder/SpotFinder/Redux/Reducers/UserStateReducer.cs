using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;
using System.Reactive;

namespace SpotFinder.Redux.Reducers
{
    public class UserStateReducer : IReducer<UserState>
    {
        private readonly IReducer<AsyncOperationState<User, Unit>> RegisterReducer;
        private readonly IReducer<AsyncOperationState<User, AccessProvider>> LoginReducer;
        private readonly IReducer<AsyncOperationState<User, object>> EditReducer;
        private readonly IReducer<User> UserReducer;

        public UserStateReducer(
            IReducer<AsyncOperationState<User, Unit>> registerReducer, 
            IReducer<AsyncOperationState<User, AccessProvider>> loginReducer, 
            IReducer<AsyncOperationState<User, object>> editReducer,
            IReducer<User> userReducer)
        {
            RegisterReducer = registerReducer ?? throw new ArgumentNullException(nameof(RegisterReducer));
            LoginReducer = loginReducer ?? throw new ArgumentNullException(nameof(LoginReducer));
            EditReducer = editReducer ?? throw new ArgumentNullException(nameof(editReducer));
            UserReducer = userReducer ?? throw new ArgumentNullException(nameof(UserReducer));
        }

        public UserState Reduce(UserState previousState, IAction action)
        {
            return new UserState(
                RegisterReducer.Reduce(previousState.Registration, action),
                LoginReducer.Reduce(previousState.Login, action),
                EditReducer.Reduce(previousState.Edit, action),
                UserReducer.Reduce(previousState.User, action)
            );
        }
    }
}
