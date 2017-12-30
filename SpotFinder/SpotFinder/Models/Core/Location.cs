using Newtonsoft.Json;

namespace SpotFinder.Models.Core
{
    public class Location
    {
        [JsonProperty("latitude")]
        public double Latitude { get; private set; }

        [JsonProperty("longitude")]
        public double Longitude { get; private set; }

        public Location() { }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
