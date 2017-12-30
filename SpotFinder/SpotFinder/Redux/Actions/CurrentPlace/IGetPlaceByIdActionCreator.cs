namespace SpotFinder.Redux.Actions.CurrentPlace
{
    public interface IGetPlaceByIdActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> GetPlaceById(int id);
    }
}
