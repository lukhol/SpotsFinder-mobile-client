using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.Users
{
    public class SetLoginCompleteAction : IAction
    {
        public User User { get; private set; }
        public Exception Error { get; private set; }
        public Status Status { get; private set; }

        public SetLoginCompleteAction(Exception error)
        {
            Error = error;
            User = null;
            Status = Status.Error;
        }

        public SetLoginCompleteAction(User user)
        {
            User = user;
            Error = null;
            Status = Status.Success;
        }
    }
}
