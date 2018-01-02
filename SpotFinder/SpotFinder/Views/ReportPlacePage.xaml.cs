using SpotFinder.Config;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportPlacePage : NavContentPage
	{
		public ReportPlacePage ()
		{
			InitializeComponent ();
            BindingContext = DIContainer.Instance.Resolve<ReportPlaceViewModel>();
		}

        private void EditorFocused(object sender, FocusEventArgs e)
        {
            MainStackLayoutXAML.HeightRequest = MainStackLayoutXAML.Height / 2;
            MainStackLayoutXAML.Layout(new Rectangle(0, 0, MainStackLayoutXAML.Width, MainStackLayoutXAML.HeightRequest));
        }

        private void EditorUnfocused(object sender, FocusEventArgs e)
        {
            MainStackLayoutXAML.HeightRequest = MainStackLayoutXAML.Height * 2;
            MainStackLayoutXAML.Layout(new Rectangle(0, 0, MainStackLayoutXAML.Width, MainStackLayoutXAML.HeightRequest));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if(BindingContext is BaseViewModel)
            {
                var baseViewModel = BindingContext as BaseViewModel;
                baseViewModel.CancelSubscriptions();
            }
        }
    }
}