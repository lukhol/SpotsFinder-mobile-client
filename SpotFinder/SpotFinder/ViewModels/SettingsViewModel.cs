using SpotFinder.Core.Enums;
using System;
using SpotFinder.Redux.Actions;
using SpotFinder.Resx;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;

namespace SpotFinder.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            App.AppStore
                .DistinctUntilChanged(state => new { state.Settings })
                .Subscribe(state =>
                {
                    Distance = state.Settings.MainDistance;
                    City = state.Settings.MainCity;
                    SelectedMapTypeString = state.Settings.MapType.ToString();
                });
        }

        private List<string> mapTypeForPickerList = new List<string>
        {
            "Normal", "Satelite"
        };
        public List<string> MapTypeForPickerList
        {
            get => mapTypeForPickerList;
            set
            {
                mapTypeForPickerList = value;
                OnPropertyChanged();
            }
        }

        private string selectedMapTypeString;
        public string SelectedMapTypeString
        {
            get => selectedMapTypeString;
            set
            {
                selectedMapTypeString = value;
                OnPropertyChanged();
            }
        }

        private double distance;
        public double Distance
        {
            get => distance;
            set
            {
                distance = value;
                distanceLabelText = AppResources.GlobalDistanceSettingsLabel + ((int)distance).ToString() + " km";
                OnPropertyChanged();
                OnPropertyChanged("DistanceLabelText");
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }

        private string distanceLabelText = AppResources.GlobalDistanceSettingsLabel + "1 km";
        public string DistanceLabelText
        {
            get => distanceLabelText;
        }

        public ICommand SaveCommad => new Command(() =>
        {
            MapType mapType;

            switch (SelectedMapTypeString)
            {
                case "Satelite":
                    mapType = MapType.Satelite;
                    break;

                case "Normal":
                    mapType = MapType.Normal;
                    break;

                default:
                    mapType = MapType.Normal;
                    break;
            }

            App.AppStore.Dispatch(new SaveSettingsAction(city, (int)distance, mapType));
            App.Current.MainPage.Navigation.PopAsync();
        });
    }
}
