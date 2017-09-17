using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Actions
{
    public class SaveSettingsAction : IAction
    {
        public string MainCity { get; private set; }
        public int MainDistance { get; private set; }
        public MapType MapType { get; private set; }

        public SaveSettingsAction(string city, int distance, MapType mapType)
        {
            MainCity = city;
            MainDistance = distance;
            MapType = mapType;
        }

        public SaveSettingsAction(Settings settings)
        {
            MainCity = settings.MainCity;
            MainDistance = settings.MainDistance;
            MapType = settings.MapType;
        }
    }
}
