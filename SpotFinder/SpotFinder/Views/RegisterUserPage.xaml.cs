using SpotFinder.Config;
using SpotFinder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterUserPage : ContentPage
	{
		public RegisterUserPage ()
		{
            BindingContext = DIContainer.Instance.Resolve<RegisterUserViewModel>();
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var registerUserViewModel = BindingContext as RegisterUserViewModel;

            if (registerUserViewModel != null)
                registerUserViewModel.OnParentPageAppearing();
        }
    }
}