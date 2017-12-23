using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    public class PassSpotToShowAction : IAction
    {
        public Place Place { get; }

        public PassSpotToShowAction(Place place)
        {
            Place = place;
        }
    }
}
