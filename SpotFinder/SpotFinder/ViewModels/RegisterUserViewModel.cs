using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Redux.StateModels;
using SpotFinder.Services;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class RegisterUserViewModel : BaseViewModel
    {
        private readonly IRegisterUserActionCreator registerUserActionCreator;
        private readonly IPhotoProvider photoProvider;

        public RegisterUserViewModel(
            IStore<ApplicationState> appStore, 
            IRegisterUserActionCreator registerUserActionCreator,
            IPhotoProvider photoProvider
            ) : base(appStore)
        {
            CancelSubscriptions();

            this.photoProvider = photoProvider ?? throw new ArgumentNullException(nameof(photoProvider));
            this.registerUserActionCreator = registerUserActionCreator ?? throw new ArgumentNullException(nameof(registerUserActionCreator));

            var registerSubscription = appStore
                .DistinctUntilChanged(state => new { state.UserState.Registration })
                .SubscribeWithError(state =>
                {
                    var registrationState = state.UserState.Registration;

                    if(registrationState.Status == Status.Setting)
                    {
                        IsBusy = true;
                        IsErrorMessageVisible = false;
                    }
                    else if(registrationState.Status == Status.Success)
                    {
                        IsBusy = false;
                        IsErrorMessageVisible = false;
                        App.Current.MainPage.Navigation.PopModalAsync();
                        App.Current.MainPage.DisplayAlert("Success", "Registration completed. You can login now.", "Ok");
                    }
                    else if(registrationState.Status == Status.Error)
                    {
                        IsBusy = false;
                        ErrorMessage = registrationState.Error.Message;
                        IsErrorMessageVisible = true;
                    }

                }, error => { appStore.Dispatch(new SetErrorAction(error, "Sub in RegisterUserViewModel.")); });
            subscriptions.Add(registerSubscription);
        }

        public void OnParentPageAppearing()
        {
            AvatarSource = ImageSource.FromUri(
                new Uri("https://lh3.googleusercontent.com/-XdUIqdMkCWA/AAAAAAAAAAI/AAAAAAAAAAA/4252rscbv5M/photo.jpg?sz=200")
            );
            Firstname = string.Empty;
            Lastname = string.Empty;
            Email = string.Empty;
            IsErrorMessageVisible = false;
        }

        private bool isImageBusy;
        public bool IsImageBusy
        {
            get => isImageBusy;
            set
            {
                isImageBusy = value;
                OnPropertyChanged();
            }
        }

        private ImageSource avatarSource;
        public ImageSource AvatarSource
        {
            get => avatarSource;
            set
            {
                avatarSource = value;
                OnPropertyChanged();
            }
        }

        private Stream avatarStream;

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

        private bool isEmailValid;
        public bool IsEmailValid
        {
            get => isEmailValid;
            set
            {
                isEmailValid = value;
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

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        private bool isErrorMessageVisible = false;
        public bool IsErrorMessageVisible
        {
            get => isErrorMessageVisible;
            set
            {
                isErrorMessageVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeAvatarCommand => new Command(ChangeAvatar);
        public ICommand RegisterUserCommand => new Command(RegisterUser);

        private async void ChangeAvatar()
        {
            IsImageBusy = true;

            var imageStream = await photoProvider.GetPhotoAsStreamAsync(Core.Enums.GetPhotoType.Camera);

            if (imageStream != null)
            {
                Stream avatarMemoryStream = new MemoryStream();
                await imageStream.CopyToAsync(avatarMemoryStream);

                imageStream.Position = 0;
                avatarMemoryStream.Position = 0;

                avatarStream = avatarMemoryStream;
                AvatarSource = ImageSource.FromStream(() => imageStream);
            }

            IsImageBusy = false;
        }

        private void RegisterUser()
        {
            if (!isEmailValid)
                return;

            User user = new User(
                id: 0,
                firstname: firstname,
                lastname: lastname,
                email: email,
                facebookId: null,
                googleId: null,
                avatarUrl: null,
                accessToken: null,
                refreshToken: null
            );

            appStore.DispatchAsync(registerUserActionCreator.Register(user, password, avatarStream));
        }
    }
}
