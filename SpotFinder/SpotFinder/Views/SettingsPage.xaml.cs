using SpotFinder.Config;
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

        protected override void OnDisappearing()
        {
            if(BindingContext is SettingsViewModel)
            {
                var settingsViewModel = BindingContext as SettingsViewModel;
                settingsViewModel.SaveCommad.Execute(null);
            }
        }
    }
}