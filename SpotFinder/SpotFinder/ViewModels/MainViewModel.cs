using Plugin.Geolocator;
using SpotFinder.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation Navigation;

        public MainViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public Command StartAddingCommand => new Command(() =>
        {
            Navigation.PushAsync(new AddingProcessPage());
        });

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
