using SpotFinder.Core.Enums;
using System;
using SpotFinder.Redux.Actions;
using SpotFinder.Resx;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels.Xaml
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
                });
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
            App.AppStore.Dispatch(new SaveSettingsAction(city, (int)distance, MapType.Normal));
            App.Current.MainPage.Navigation.PopAsync();
        });
    }
}
