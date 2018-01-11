﻿using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;
using System.Reactive;

namespace SpotFinder.Redux.Reducers
{
    public class UserStateReducer : IReducer<UserState>
    {
        private readonly IReducer<AsyncOperationState<User, AccessProvider>> RegisterReducer;
        private readonly IReducer<AsyncOperationState<User, Unit>> LoginReducer;
        private readonly IReducer<User> UserReducer;

        public UserStateReducer(
            IReducer<AsyncOperationState<User, AccessProvider>> registerReducer, 
            IReducer<AsyncOperationState<User, Unit>> loginReducer, 
            IReducer<User> userReducer)
        {
            RegisterReducer = registerReducer ?? throw new ArgumentNullException(nameof(RegisterReducer));
            LoginReducer = loginReducer ?? throw new ArgumentNullException(nameof(LoginReducer));
            UserReducer = userReducer ?? throw new ArgumentNullException(nameof(UserReducer));
        }

        public UserState Reduce(UserState previousState, IAction action)
        {
            return new UserState(
                RegisterReducer.Reduce(previousState.Registration, action),
                LoginReducer.Reduce(previousState.Login, action),
                UserReducer.Reduce(previousState.User, action)
            );
        }
    }
}