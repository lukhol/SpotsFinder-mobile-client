using SpotFinder.Config;
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
            FocusSetup();
        }

        private void FocusSetup()
        {
            NameEntryXaml.Completed += (s, e) =>
            {
                DescriptionEntryXaml.Focus();
            };

            DescriptionEntryXaml.Completed += (s, e) =>
            {
                TypePickerXaml.Focus();
            };

        }
    }
}