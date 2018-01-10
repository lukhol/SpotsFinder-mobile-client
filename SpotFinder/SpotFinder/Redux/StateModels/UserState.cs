using SpotFinder.Core.Enums;
using System;
using System.Reactive;

namespace SpotFinder.Redux.StateModels
{
    public class UserState
    {
        public AsyncOperationState<User, AccessProvider> Registration { get; private set; }
        public AsyncOperationState<User, Unit> Login { get; private set; }

        public User User { get; private set; }

        [Obsolete]
        public UserState() { }

        public UserState(
            AsyncOperationState<User, AccessProvider> registration,
            AsyncOperationState<User, Unit> login,
            User user
            )
        {
            Registration = registration;
            Login = login;
            User = user;
        }
    }
}
