using Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Redux.Actions
{
    class UpdateDeviceLocationAction : IAction
    {
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        public UpdateDeviceLocationAction(double longitude, double latitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
