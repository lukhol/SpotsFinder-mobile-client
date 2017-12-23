using Redux;

namespace SpotFinder.Redux.Actions
{
    public class UpdateLocationInReportAction : IAction
    {
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        public UpdateLocationInReportAction(double longitude, double latitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
