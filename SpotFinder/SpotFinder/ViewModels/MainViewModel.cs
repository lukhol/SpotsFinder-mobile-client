using SpotFinder.Views;
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
            message = "SpotsFinder!";
            Navigation = navigation;
        }

        private string message;

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public Command MessageCommand => new Command(() => 
        {
            Message += ", message";
        });

        public Command GoNextCommand => new Command(() =>
        {
            Navigation.PushAsync(new ListPage());
        });

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
