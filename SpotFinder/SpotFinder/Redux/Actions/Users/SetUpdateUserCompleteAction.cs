using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Redux.Actions.Users
{
    public class SetUpdateUserCompleteAction : IAction
    {
        public User User { get; private set; }
        public Exception Error { get; private set; }
        public Status Status { get; private set; }

        public SetUpdateUserCompleteAction(User user)
        {
            User = user;
            Error = null;
            Status = Status.Success;
        }

        public SetUpdateUserCompleteAction(Exception error)
        {
            Error = error;
            User = null;
            Status = Status.Error;
        }
    }
}
