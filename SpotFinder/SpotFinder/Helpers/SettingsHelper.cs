using System;
using SpotFinder.Redux.StateModels;
using SpotFinder.Core.Enums;

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

            if (App.Current.Properties.ContainsKey("MapType"))
            {
                var mapType = App.Current.Properties["MapType"] as string;
                settings.MapType = (MapType)Enum.Parse(typeof(MapType), mapType);
            }           

            return settings;
        }

        public async void SaveSettingsAsync(Settings settings)
        {
            App.Current.Properties["MainCity"] = settings.MainCity;
            App.Current.Properties["MainDistance"] = settings.MainDistance;
            App.Current.Properties["MapType"] = settings.MapType.ToString();

            await App.Current.SavePropertiesAsync();
        }
    }
}
