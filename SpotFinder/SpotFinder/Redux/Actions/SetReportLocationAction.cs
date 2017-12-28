using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    public class SetReportLocationAction : IAction
    {
        public Location Location { get; private set; }

        public SetReportLocationAction(Location location)
        {
            Location = location;
        }
    }
}
