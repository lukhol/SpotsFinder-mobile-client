using SpotFinder.Config;
using SpotFinder.Services;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocateOnMapPage : NavContentPage
    {
        public LocateOnMapPage()
        {
            InitializeComponent();
            BindingContext = DIContainer.Instance.Resolve<LocateOnMapViewModel>();
        }

        protected override bool OnBackButtonPressed()
        {
            var locateViewModel = (BaseViewModel)BindingContext;

            if (!locateViewModel.IsBusy)
                locateViewModel.GoBackCommand.Execute(null);

            return true;
        }

        public override bool OnNavigationBackButtonPressed()
        {
            var locateViewModel = (BaseViewModel)BindingContext;

            if (!locateViewModel.IsBusy)
                locateViewModel.GoBackCommand.Execute(null);

            return true;
        }
    }
}