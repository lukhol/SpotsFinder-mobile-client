using SpotFinder.Redux.StateModels;
using System.IO;

namespace SpotFinder.Redux.Actions.Users
{
    public interface IRegisterUserActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> Register(User user, Stream avatarStream);
    }
}
