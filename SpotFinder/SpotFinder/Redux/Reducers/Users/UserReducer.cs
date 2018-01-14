using BuilderImmutableObject;
using Redux;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers.Users
{
    public class UserReducer : IReducer<User>
    {
        public User Reduce(User previousState, IAction action)
        {
            if(action is SetLoggedInUserAction)
            {
                var setLoggedInUserAction = action as SetLoggedInUserAction;
                return setLoggedInUserAction.User;
            }

            if(action is SetUserAvatarUrlAction)
            {
                var setUserAvatarUrlAction = action as SetUserAvatarUrlAction;
                return previousState
                     .Set(v => v.AvatarUrl, setUserAvatarUrlAction.AvatarUrl)
                     .Build();
            }

            return previousState;
        }
    }
}
