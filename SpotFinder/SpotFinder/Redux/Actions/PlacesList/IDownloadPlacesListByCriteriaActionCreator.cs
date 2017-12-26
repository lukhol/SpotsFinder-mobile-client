using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public interface IDownloadPlacesListByCriteriaActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlaceByCriteria(Criteria criteria);
    }
}
