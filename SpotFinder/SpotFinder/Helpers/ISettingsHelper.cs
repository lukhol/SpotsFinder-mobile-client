using SpotFinder.Redux.StateModels;

namespace SpotFinder.Helpers
{
    public interface ISettingsHelper
    {
        void SaveSettings(Settings settings);
        Settings ReadSettings();
    }
}
