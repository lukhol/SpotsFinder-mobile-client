using Redux;
using SpotFinder.Helpers;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Reducers
{
    public class SettingsReducer : IReducer<Settings>
    {
        private ISettingsHelper SettingsHelper { get; }

        public SettingsReducer(ISettingsHelper settingsHelper)
        {
            SettingsHelper = settingsHelper ?? throw new ArgumentNullException("settingsHelper is null in SettingsReducer");
        }

        public Settings Reduce(Settings previousState, IAction action)
        {
            if(action is SaveSettingsAction)
            {
                var saveSettingsAction = action as SaveSettingsAction;

                return new Settings(
                    saveSettingsAction.MainCity,
                    saveSettingsAction.MainDistance,
                    saveSettingsAction.MapType,
                    saveSettingsAction.FirstUse
                );
            }

            if(action is ReadSettingsAction)
            {
                var readSettingsAction = action as ReadSettingsAction;

                return new Settings(
                    readSettingsAction.MainCity,
                    readSettingsAction.MainDistance,
                    readSettingsAction.MapType,
                    readSettingsAction.FirstUse
                );
            }

            return previousState;
        }
    }
}
