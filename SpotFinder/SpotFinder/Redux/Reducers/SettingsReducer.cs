using SpotFinder.Redux.StateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Helpers;

namespace SpotFinder.Redux.Reducers
{
    public class SettingsReducer : IReducer<Settings>
    {
        private ISettingsHelper SettingsHelper { get; }

        public SettingsReducer(ISettingsHelper settingsHelper)
        {
            SettingsHelper = settingsHelper ?? throw new ArgumentNullException("settingsHelper is null in SettingsReducer");
        }

        public Settings Reduce(Settings localState, IAction action)
        {
            if(action is SaveSettingsAction)
            {
                var saveSettingsAction = action as SaveSettingsAction;
                var settings = new Settings
                {
                    MainCity = saveSettingsAction.MainCity,
                    MainDistance = saveSettingsAction.MainDistance,
                    MapType = saveSettingsAction.MapType
                };

                SettingsHelper.SaveSettingsAsync(settings);

                return localState;
            }

            if(action is ReadSettingsAction)
            {
                var readSettingsAction = action as ReadSettingsAction;

                return new Settings
                {
                    MainCity = readSettingsAction.MainCity,
                    MainDistance = readSettingsAction.MainDistance,
                    MapType = readSettingsAction.MapType
                };
            }

            return localState;
        }
    }
}
