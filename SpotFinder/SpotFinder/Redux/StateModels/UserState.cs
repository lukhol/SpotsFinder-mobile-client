﻿using SpotFinder.Core.Enums;
using System;
using System.Reactive;

namespace SpotFinder.Redux.StateModels
{
    public class UserState
    {
        public AsyncOperationState<User, Unit> Registration { get; private set; }
        public AsyncOperationState<User, AccessProvider> Login { get; private set; }

        public User User { get; private set; }

        [Obsolete]
        public UserState() { }

        public UserState(
            AsyncOperationState<User, Unit> registration,
            AsyncOperationState<User, AccessProvider> login,
            User user
            )
        {
            Registration = registration;
            Login = login;
            User = user;
        }
    }
}
