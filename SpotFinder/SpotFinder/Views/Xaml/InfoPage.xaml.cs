using SpotFinder.ViewModels.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Xaml
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
            BindingContext = new InfoViewModel();

            if(Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.WinPhone)
            {
                LogoImage.WidthRequest = 150;
                LogoImage.HeightRequest = 150;
                LogoImage.Aspect = Aspect.AspectFill;
            }
        }
    }
}