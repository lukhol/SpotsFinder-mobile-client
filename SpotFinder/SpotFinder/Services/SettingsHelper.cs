using System;
using SpotFinder.Redux.StateModels;
using SpotFinder.Core.Enums;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SpotFinder.Helpers
{
    public class SettingsHelper : ISettingsHelper
    {
        private ISettings currentSettings;

        public SettingsHelper()
        {
            currentSettings = CrossSettings.Current;
        }

        public Settings ReadSettings()
        {
            return new Settings(
                currentSettings.GetValueOrDefault("MainCity", "łódź"),
                currentSettings.GetValueOrDefault("MainDistance", 10),
                (MapType)Enum.Parse(typeof(MapType), currentSettings.GetValueOrDefault("MapType", "Satelite")),
                currentSettings.GetValueOrDefault("FirstUse", false)
            );
        }

        public void SaveSettings(Settings settings)
        {
            currentSettings.AddOrUpdateValue("MainCity", settings.MainCity);
            currentSettings.AddOrUpdateValue("MainDistance", settings.MainDistance);
            currentSettings.AddOrUpdateValue("MapType", settings.MapType.ToString());
            currentSettings.AddOrUpdateValue("FirstUse", settings.FirstUse);
        }
    }
}
