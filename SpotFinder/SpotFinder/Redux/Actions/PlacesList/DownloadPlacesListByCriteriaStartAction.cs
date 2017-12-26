using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class DownloadPlacesListByCriteriaStartAction : IAction
    {
        public Criteria Criteria { get; private set; }

        public DownloadPlacesListByCriteriaStartAction(Criteria criteria)
        {
            Criteria = criteria;
        }
    }
}
