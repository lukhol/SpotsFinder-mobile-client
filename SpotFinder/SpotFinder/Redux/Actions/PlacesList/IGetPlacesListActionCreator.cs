using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public interface IGetPlacesListActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlacesByCriteria(Criteria criteria);
        StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlacesByUserId(long userId);
    }
}
