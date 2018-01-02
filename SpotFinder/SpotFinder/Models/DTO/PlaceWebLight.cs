using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;

namespace SpotFinder.Models.DTO
{
    public class PlaceWebLight
    {
        public int Id { get; set; }
        public long Version { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PlaceType Type { get; set; }
        public string MainPhoto { get; set; }
    }
}
