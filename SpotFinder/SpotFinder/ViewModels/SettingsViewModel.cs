﻿using SpotFinder.Core.Enums;
using System;
using SpotFinder.Redux.Actions;
using SpotFinder.Resx;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using SpotFinder.Redux;
using Redux;

namespace SpotFinder.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private const int MAX_CITY_LENGTH = 85;
        private const int MIN_CITY_LENGTH = 1;

        public SettingsViewModel(IStore<ApplicationState> appStore) : base(appStore)
        {
            appStore
                .DistinctUntilChanged(state => new { state.Settings.MainDistance })
                .Subscribe(state => Distance = state.Settings.MainDistance );

            appStore
                .DistinctUntilChanged(state => new { state.Settings.MainCity })
                .Subscribe(state => City = state.Settings.MainCity);

            appStore
                .DistinctUntilChanged(state => new { state.Settings.MapType })
                .Subscribe(state => SelectedMapTypeString = state.Settings.MapType.ToString());
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

            appStore.Dispatch(new SaveSettingsAction(city, (int)distance, mapType));
            App.Current.MainPage.Navigation.PopAsync();
        });
    }
}
