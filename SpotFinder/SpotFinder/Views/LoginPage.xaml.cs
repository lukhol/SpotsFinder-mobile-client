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
            FocusSetup();
        }

        protected override void OnAppearing()
        {
            var loginViewModel = BindingContext as LoginViewModel;
            if (loginViewModel != null)
                loginViewModel.WebView = WebViewXaml;

            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            var loginViewModel = BindingContext as LoginViewModel;
            if (loginViewModel != null)
            {
                if (loginViewModel.IsWebViewVisible)
                {
                    loginViewModel.IsWebViewVisible = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        private void FocusSetup()
        {
            EmailEntryXaml.Completed += (s, e) => PasswordEntryXaml.Focus();
        }
    }
}