using Redux;

namespace SpotFinder.Redux.Actions.Users
{
    public class SetUserAvatarUrlAction : IAction
    {
        public string AvatarUrl { get; private set; }

        public SetUserAvatarUrlAction(string avatarUrl)
        {
            AvatarUrl = avatarUrl;
        }
    }
}
