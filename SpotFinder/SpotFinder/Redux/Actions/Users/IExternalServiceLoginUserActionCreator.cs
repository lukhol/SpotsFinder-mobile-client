namespace SpotFinder.Redux.Actions.Users
{
    public interface IExternalServiceLoginUserActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> Login(string uri);
    }
}
