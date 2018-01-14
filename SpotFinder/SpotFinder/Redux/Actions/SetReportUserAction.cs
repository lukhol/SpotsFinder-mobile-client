using Redux;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Actions
{
    public class SetReportUserAction : IAction
    {
        public User User { get; private set; }

        public SetReportUserAction(User user)
        {
            User = user;
        }
    }
}
