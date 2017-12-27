using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    public class UpdateLocationInReportAction : IAction
    {
        public Location Location { get; private set; }

        public UpdateLocationInReportAction(Location location)
        {
            Location = location;
        }
    }
}
