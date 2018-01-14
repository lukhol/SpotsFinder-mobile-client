using SpotFinder.Config;
using SpotFinder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserDetailsPage : ContentPage
	{
		public UserDetailsPage ()
		{
			InitializeComponent ();
            BindingContext = DIContainer.Instance.Resolve<UserDetailsViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var userDetailsViewModel = BindingContext as UserDetailsViewModel;
            if(userDetailsViewModel != null)
            {
                userDetailsViewModel.AvatarFFCachedImage = AvatarXaml;
            }
        }
    }
}