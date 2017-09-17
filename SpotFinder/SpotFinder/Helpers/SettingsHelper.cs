using System;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Helpers
{
    public class SettingsHelper : ISettingsHelper
    {
        public Settings ReadSettings()
        {
            var settings = new Settings();

            if (App.Current.Properties.ContainsKey("MainCity"))
                settings.MainCity = App.Current.Properties["MainCity"] as string;

            if (App.Current.Properties.ContainsKey("MainDistance"))
                settings.MainDistance = (int)App.Current.Properties["MainDistance"];


            return settings;
        }

        public async void SaveSettingsAsync(Settings settings)
        {
            App.Current.Properties["MainCity"] = settings.MainCity;
            App.Current.Properties["MainDistance"] = settings.MainDistance;

            await App.Current.SavePropertiesAsync();
        }
    }
}
