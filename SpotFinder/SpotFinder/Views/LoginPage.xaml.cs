using SpotFinder.Config;
using SpotFinder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
            BindingContext = DIContainer.Instance.Resolve<LoginViewModel>();
			InitializeComponent ();
        }

        protected override void OnAppearing()
        {
            var loginViewModel = BindingContext as LoginViewModel;
            if (loginViewModel != null)
                loginViewModel.WebView = WebViewXaml;

            base.OnAppearing();
        }
    }
}