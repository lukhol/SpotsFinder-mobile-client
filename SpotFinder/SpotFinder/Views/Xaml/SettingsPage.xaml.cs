using SpotFinder.Resx;
using SpotFinder.ViewModels.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Xaml
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }
    }
}