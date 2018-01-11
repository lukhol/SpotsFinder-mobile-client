using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.Actions.Users
{
    public interface IExternalServiceLoginUserActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> Login(string uri, AccessProvider accessProvider);
    }
}
