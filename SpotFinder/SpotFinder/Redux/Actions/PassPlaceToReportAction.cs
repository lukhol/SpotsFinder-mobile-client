using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    public class PassPlaceToReportAction : IAction
    {
        public Place Place { get; }

        public PassPlaceToReportAction(Place place)
        {
            Place = place;
        }
    }
}
