using Redux;
using SpotFinder.Redux;
using SpotFinder.Views.Root;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(IStore<ApplicationState> appStore) : base(appStore)
        {

        }

        private string username = string.Empty;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        private string password = string.Empty;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        private bool isVisibleWrongCredentialLabel;
        public bool IsVisibleWrongCredentialLabel
        {
            get => isVisibleWrongCredentialLabel;
            set
            {
                isVisibleWrongCredentialLabel = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand => new Command(Login);
        public ICommand SkipLoginCommand => new Command(SkipLogin);
        public ICommand RegisterCommand => new Command(Register);

        private async void Login()
        {
            if (username.Equals("admin") && username.Equals("admin"))
                await App.Current.MainPage.Navigation.PopModalAsync();
            else
                IsVisibleWrongCredentialLabel = true;
        }

        private async void SkipLogin()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void Register()
        {
            //await App.Current.MainPage.Navigation.PushModalAsync(new RegisterUserPage());
        }
    }
}
