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
                var settings = new Settings
                {
                    MainCity = saveSettingsAction.MainCity,
                    MainDistance = saveSettingsAction.MainDistance,
                    MapType = saveSettingsAction.MapType
                };

                SettingsHelper.SaveSettingsAsync(settings);

                //Nie mogę zwrócić nowego stanu ponieważ na zmianę Settings jest subskrybcja, która zmienia je w stanie.
                //Dlatego zwracam stary stan, ale z nowymi danymi. Nie wiem czy to dobrze, ale dzięki temu mogę zapisać
                //się również na subskrybcję dla poszczególnych części Settingsów np. MainCity lub MapType.
                previousState.MainCity = settings.MainCity;
                previousState.MainDistance = settings.MainDistance;
                previousState.MapType = settings.MapType;

                return previousState;
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

            return previousState;
        }
    }
}
