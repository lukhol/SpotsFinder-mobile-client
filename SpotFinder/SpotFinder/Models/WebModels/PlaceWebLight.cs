using SpotFinder.Core;
using SpotFinder.Core.Enums;

namespace SpotFinder.Models.WebModels
{
    public class PlaceWebLight
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PlaceType Type { get; set; }
        public string MainPhoto { get; set; }
    }
}
