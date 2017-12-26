using SpotFinder.Config;
using SpotFinder.Services;
using SpotFinder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
            BindingContext = DIContainer.Instance.Resolve<InfoViewModel>();
  
            if (Device.RuntimePlatform == Device.WinRT || Device.RuntimePlatform == Device.WinPhone)
            {
                LogoImage.WidthRequest = 150;
                LogoImage.HeightRequest = 150;
                LogoImage.Aspect = Aspect.AspectFill;
            }
        }
    }
}