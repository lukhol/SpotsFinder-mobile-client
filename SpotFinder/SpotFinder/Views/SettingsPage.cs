using Microsoft.Practices.ServiceLocation;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder.Views
{
    public class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            Title = "Settings";
            var settingsViewModel = ServiceLocator.Current.GetInstance<SettingsViewModel>();
            settingsViewModel.InjectPage(this);
            BindingContext = settingsViewModel;
            settingsViewModel.CheckProperties();
        }
    }
}