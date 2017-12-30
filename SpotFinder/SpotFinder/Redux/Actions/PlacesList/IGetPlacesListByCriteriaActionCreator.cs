using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public interface IGetPlacesListByCriteriaActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlaceByCriteria(Criteria criteria);
    }
}
