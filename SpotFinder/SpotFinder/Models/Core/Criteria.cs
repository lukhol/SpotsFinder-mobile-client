using SpotFinder.Core.Enums;
using System.Collections.Generic;

namespace SpotFinder.Models.Core
{
    public class Criteria
    {
        public List<PlaceType> Type { get; private set; }
        public CityLocation Location { get; private set; }
        public int Distance { get; private set; }
        public bool Gap { get; private set; }
        public bool Stairs { get; private set; }
        public bool Rail { get; private set; }
        public bool Ledge { get; private set; }
        public bool Handrail { get; private set; }
        public bool Corners { get; private set; }
        public bool Manualpad { get; private set; }
        public bool Wallride { get; private set; }
        public bool Downhill { get; private set; }
        public bool OpenYourMind { get; private set; }
        public bool Pyramid { get; private set; }
        public bool Curb { get; private set; }
        public bool Bank { get; private set; }
        public bool Bowl { get; private set; }
        public bool Hubba { get; private set; }

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

        public Criteria(List<PlaceType> type, CityLocation location, int distance)
        {
            Type = type;
            Location = location;
            Distance = distance;
        }

        public Criteria(
            List<PlaceType> type, 
            CityLocation location, 
            int distance, 
            bool gap,
            bool stairs, 
            bool rail, 
            bool ledge, 
            bool handrail, 
            bool corners, 
            bool manualpad, 
            bool wallride, 
            bool downhill, 
            bool openYourMind,
            bool pyramid,
            bool curb, 
            bool bank, 
            bool bowl, 
            bool hubba)
        {
            Type = type;
            Location = location;
            Distance = distance;
            Gap = gap;
            Stairs = stairs;
            Rail = rail;
            Ledge = ledge;
            Handrail = handrail;
            Corners = corners;
            Manualpad = manualpad;
            Wallride = wallride;
            Downhill = downhill;
            OpenYourMind = openYourMind;
            Pyramid = pyramid;
            Curb = curb;
            Bank = bank;
            Bowl = bowl;
            Hubba = hubba;
        }
    }
}
