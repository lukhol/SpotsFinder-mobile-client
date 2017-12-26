using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions.CurrentPlace
{
    class DownloadPlaceByIdCompleteAction : IAction
    {
        public Place Place { get; private set; }

        public DownloadPlaceByIdCompleteAction(Place place)
        {
            Place = place;
        }
    }
}
