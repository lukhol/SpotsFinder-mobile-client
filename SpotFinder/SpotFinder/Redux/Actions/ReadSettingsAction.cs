using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Actions
{
    public class ReadSettingsAction : IAction
    {
        public string MainCity { get; private set; }
        public int MainDistance { get; private set; }
        public MapType MapType { get; private set; }
        public bool FirstUse { get; private set; }

        public ReadSettingsAction(Settings settings)
        {
            MainCity = settings.MainCity;
            MainDistance = settings.MainDistance;
            MapType = settings.MapType;
            FirstUse = settings.FirstUse;
        }
    }
}
