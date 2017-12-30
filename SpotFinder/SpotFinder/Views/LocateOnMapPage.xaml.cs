using SpotFinder.Config;
using SpotFinder.Services;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using System;
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

        public void UploadPlace(object sender, EventArgs e)
        {
            if (!(BindingContext is LocateOnMapViewModel))
                return;

            var locateOnMapViewModel = BindingContext as LocateOnMapViewModel;
            locateOnMapViewModel.UploadPlaceCommand.Execute(MapXAML);
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