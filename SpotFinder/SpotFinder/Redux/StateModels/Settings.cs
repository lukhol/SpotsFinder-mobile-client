using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.StateModels
{
    public class Settings
    {
        public string MainCity { get; private set; }
        public int MainDistance { get; private set; }
        public MapType MapType { get; private set; }
        public bool FirstUse { get; private set; }

        public Settings() { }

        public Settings(string mainCity, int mainDistance, MapType mapType, bool firstUse)
        {
            MainCity = mainCity;
            MainDistance = mainDistance;
            MapType = mapType;
            FirstUse = firstUse;
        }
    }
}
