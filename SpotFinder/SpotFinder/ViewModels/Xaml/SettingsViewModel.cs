using SpotFinder.Redux;
using SpotFinder.Resx;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels.Xaml
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            distance = App.AppStore.GetState().GlobalDistance;
             city = App.AppStore.GetState().MainCity;
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
            App.AppStore.Dispatch(new SaveSettingsAction(city, (int)distance));
        });
    }
}
