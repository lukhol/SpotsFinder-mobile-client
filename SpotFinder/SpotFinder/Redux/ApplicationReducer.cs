using Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Redux
{
    public class ApplicationReducer
    {
        public static ApplicationState Reducer(ApplicationState state, IAction action)
        {
            if(action is InitAction)
            {
                var initAction = (InitAction)action;
                ReadSettings(state);
            }

            if(action is DownloadSinglePlaceAction)
            {
                var downloadSinglePlaceAction = (DownloadSinglePlaceAction)action;
                state.RequestDownloadPlace(downloadSinglePlaceAction.Id);
            }

            if(action is SaveSettingsAction)
            {
                var saveSettingsAction = (SaveSettingsAction)action;
                SaveSettingsAsync(saveSettingsAction.City, saveSettingsAction.Distance);
                state.MainCity = saveSettingsAction.City;
                state.GlobalDistance = saveSettingsAction.Distance;
                App.Current.MainPage.Navigation.PopAsync();
            }

            return state;
        }

        private static async void SaveSettingsAsync(string city, int distance)
        {
            App.Current.Properties["MainCity"] = city;
            App.Current.Properties["MainDistance"] = distance;

            await App.Current.SavePropertiesAsync();
        }

        private static void ReadSettings(ApplicationState state)
        {
            if (App.Current.Properties.ContainsKey("MainCity"))
                state.MainCity = App.Current.Properties["MainCity"] as string;

            if (App.Current.Properties.ContainsKey("MainDistance"))
                state.GlobalDistance = (int)App.Current.Properties["MainDistance"];
        }
    }
}
