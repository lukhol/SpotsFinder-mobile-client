namespace SpotFinder.Redux.Actions.CurrentPlace
{
    public interface IDownloadPlaceByIdActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlaceById(int id);
    }
}
