using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using System.Collections.Generic;

namespace SpotFinder.Models.DTO
{
    public class PlaceWeb
    {
        public int? Id { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PlaceType Type { get; set; }
        public IList<ImageWeb> Images { get; set; }

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
    }
}
