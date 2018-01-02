using SpotFinder.Core.Enums;
using System.Collections.Generic;

namespace SpotFinder.Models.Core
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Version { get; set; }
        public Location Location { get; set; }
        public PlaceType Type { get; set; }
        public IList<string> PhotosBase64List { get; set; }

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
        public bool Hubba { get; set; }

        public Place()
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
            Description = string.Empty;
            Name = string.Empty;
            Location = new Location();
            PhotosBase64List = new List<string>();
        }
    }
}
