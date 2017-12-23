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
                settings.MapType = StringToMapType(App.Current.Properties["MapType"] as string);

            return settings;
        }

        public async void SaveSettingsAsync(Settings settings)
        {
            App.Current.Properties["MainCity"] = settings.MainCity;
            App.Current.Properties["MainDistance"] = settings.MainDistance;
            App.Current.Properties["MapType"] = MapTypeToString(settings.MapType);

            await App.Current.SavePropertiesAsync();
        }

        private string MapTypeToString(MapType mapType)
        {
            switch (mapType)
            {
                case MapType.Normal:
                    return "Normal";
                case MapType.Satelite:
                    return "Satelite";
                default:
                    return string.Empty;
            }
        }

        private MapType StringToMapType(string stringMapType)
        {
            switch (stringMapType)
            {
                case "Satelite":
                    return MapType.Satelite;

                case "Normal":
                    return MapType.Normal;

                default:
                    return MapType.NoSpecified;
            }
        }
    }
}
