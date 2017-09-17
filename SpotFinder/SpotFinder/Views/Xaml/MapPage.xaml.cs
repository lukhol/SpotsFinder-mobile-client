using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Xaml
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.Xaml.MapViewModel();
        }
    }
}