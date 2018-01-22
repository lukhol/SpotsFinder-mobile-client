using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.Users
{
    public class SetUpdateUserStartAction : IAction
    {
        public User User { get; private set; }
        public Status Status { get; private set; }
        public Exception Error { get; private set; }

        public SetUpdateUserStartAction()
        {
            User = null;
            Status = Status.Setting;
            Error = null;
        }
    }
}
