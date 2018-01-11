using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.Users
{
    public class SetLoginStartAction : IAction
    {
        public Status Status { get; private set; }
        public Exception Error { get; private set; }
        public User User { get; private set; }
        public AccessProvider AccessProvider { get; private set; }

        public SetLoginStartAction(AccessProvider accessProvider)
        {
            AccessProvider = accessProvider;
            Status = Status.Setting;
            User = null;
            Error = null;
        }
    }
}
