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

            //Windows bugs...
            if(Device.RuntimePlatform == Device.Windows)
            {
                var globalDistance = App.AppStore.GetState().GlobalDistance;

                DistanceLabel.Text =
                    AppResources.GlobalDistanceSettingsLabel
                    + globalDistance.ToString()
                    + " km";

                DistanceSlider.Value = globalDistance;
            }
        }
    }
}