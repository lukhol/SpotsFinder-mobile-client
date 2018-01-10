namespace SpotFinder.Redux.Actions.Users
{
    public interface ILoginUserActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> Login(string email, string password);
    }
}
