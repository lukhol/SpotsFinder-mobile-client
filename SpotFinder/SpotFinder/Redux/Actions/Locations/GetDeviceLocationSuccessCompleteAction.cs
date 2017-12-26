using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions.Locations
{
    public class GetDeviceLocationSuccessCompleteAction : IAction
    {
        public Location Location { get; private set; }

        public GetDeviceLocationSuccessCompleteAction(Location location)
        {
            Location = location;
        }
    }
}
