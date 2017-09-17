using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.StateModels
{
    public class Settings
    {
        public string MainCity { get; set; }
        public int MainDistance { get; set; }
        public MapType MapType { get; set; }
    }
}
