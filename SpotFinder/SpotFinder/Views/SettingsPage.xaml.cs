using SpotFinder.Services;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : NavContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = DIContainer.Instance.Resolve<SettingsViewModel>();
        }
    }
}