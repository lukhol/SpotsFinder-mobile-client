using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.Users
{
    public class SetLoginStartAction : IAction
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public Status Status { get; private set; }
        public Exception Error { get; private set; }
        public User User { get; private set; }

        public SetLoginStartAction(string email, string password)
        {
            Email = email;
            Password = password;
            Status = Status.Setting;
            User = null;
            Error = null;
        }
    }
}
