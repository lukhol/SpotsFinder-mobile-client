using Newtonsoft.Json;
using SpotFinder.Core.Enums;
using System.Collections.Generic;

namespace SpotFinder.Core
{
    public class Criteria
    {
        public List<PlaceType> Type { get; set; }
        //public Location Location { get; set; }
        public CityLocation Location { get; set; }
        public int Distance { get; set; }
        public bool Gap { get; set; }
        public bool Stairs { get; set; }
        public bool Rail { get; set; }
        public bool Ledge { get; set; }
        public bool Handrail { get; set; }
        public bool Corners { get; set; }
        public bool Manualpad { get; set; }
        public bool Wallride { get; set; }
        public bool Downhill { get; set; }
        public bool OpenYourMind { get; set; }
        public bool Pyramid { get; set; }
        public bool Curb { get; set; }
        public bool Bank { get; set; }
        public bool Bowl { get; set; }
        [JsonIgnore]
        public bool Hubba { get; set; }

        public Criteria()
        {
            Gap = false;
            Stairs = false;
            Rail = false;
            Ledge = false;
            Handrail = false;
            Corners = false;
            Manualpad = false;
            Wallride = false;
            Downhill = false;
            OpenYourMind = false;
            Pyramid = false;
            Curb = false;
            Bank = false;
            Bowl = false;
            Location = new CityLocation();
            Type = new List<PlaceType>();
            Distance = 15;
        }
    }
}
