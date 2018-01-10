using Redux;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Actions.Users
{
    public class SetLoggedInUserAction : IAction
    {
        public User User { get; private set; }

        public SetLoggedInUserAction(User user)
        {
            User = user;
        }
    }
}
