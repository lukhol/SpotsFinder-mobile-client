using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.Users
{
    public class SetRegisterUserStartAction : IAction
    {
        public User User { get; private set; }
        public Status Status { get; private set; }
        public Exception Error { get; private set; }

        public SetRegisterUserStartAction(User user)
        {
            User = user;
            Status = Status.Setting;
            Error = null;
        }
    }
}
