using SpotFinder.Services;
using SpotFinder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingProcessPage : ContentPage
    {
        public AddingProcessPage()
        {
            InitializeComponent();
            BindingContext = DIContainer.Instance.Resolve<AddingProcessViewModel>();
        }
    }
}