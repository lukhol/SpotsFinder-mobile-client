using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    public class SetReportPlaceAction : IAction
    {
        public Place Place { get; }

        public SetReportPlaceAction(Place place)
        {
            Place = place;
        }
    }
}
