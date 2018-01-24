using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    public class SetUpdateReportPlaceAction : IAction
    {
        public Place Place { get; private set; }

        public SetUpdateReportPlaceAction(Place place)
        {
            Place = place;
        }
    }
}
