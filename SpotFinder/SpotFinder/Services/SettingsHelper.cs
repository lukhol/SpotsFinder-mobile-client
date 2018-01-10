using System;
using SpotFinder.Redux.StateModels;
using SpotFinder.Core.Enums;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Newtonsoft.Json;

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

        public User ReadUser()
        {
            var jsonUser = currentSettings.GetValueOrDefault("User", string.Empty);

            if (string.IsNullOrEmpty(jsonUser))
                return null;

            var user = JsonConvert.DeserializeObject<User>(jsonUser);
            return user;
        }

        public void SaveUser(User user)
        {
            if (user == null)
                currentSettings.AddOrUpdateValue("User", string.Empty);

            var userJson = JsonConvert.SerializeObject(user);
            currentSettings.AddOrUpdateValue("User", userJson);
        }
    }
}
