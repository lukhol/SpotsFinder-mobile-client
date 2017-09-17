using SpotFinder.Redux.StateModels;

namespace SpotFinder.Helpers
{
    public interface ISettingsHelper
    {
        void SaveSettingsAsync(Settings settings);
        Settings ReadSettings();
    }
}
