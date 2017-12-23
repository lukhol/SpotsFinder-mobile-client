using Redux;

namespace SpotFinder.Redux.Actions
{
    public class PassLocationToReportingPlaceAction : IAction
    {
        public double? Latitude { get; }
        public double? Longitude { get; }

        public PassLocationToReportingPlaceAction()
        {
            Latitude = null;
            Longitude = null;
        }

        public PassLocationToReportingPlaceAction(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
