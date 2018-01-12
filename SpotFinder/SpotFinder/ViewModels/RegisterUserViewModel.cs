using Redux;
using SpotFinder.Redux;

namespace SpotFinder.ViewModels
{
    public class RegisterUserViewModel : BaseViewModel
    {
        public RegisterUserViewModel(IStore<ApplicationState> appStore) : base(appStore)
        {

        }

        private string email = string.Empty;
        public string Email
        {
            get => email;
            set
            {
                email = value;
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

        private string firstname = string.Empty;
        public string Firstname
        {
            get => firstname;
            set
            {
                firstname = value;
                OnPropertyChanged();
            }
        }

        private string lastname = string.Empty;
        public string Lastname
        {
            get => lastname;
            set
            {
                lastname = value;
                OnPropertyChanged();
            }
        }
    }
}
