using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Views;
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly string FacebookLoginUrl;
        private readonly string GoogleLoginUrl;

        private readonly ILoginUserActionCreator loginUserActionCreator;
        private readonly IExternalServiceLoginUserActionCreator externalServiceLoginUserActionCreator;

        private AccessProvider LoginWith;

        public LoginViewModel(
            IStore<ApplicationState> appStore, 
            string googleLoginUrl, 
            string facebookLoginUrl,
            IExternalServiceLoginUserActionCreator facebookLoginUserActionCreator,
            ILoginUserActionCreator loginUserActionCreator
            ) : base(appStore)
        {
            GoogleLoginUrl = googleLoginUrl;
            FacebookLoginUrl = facebookLoginUrl;

            this.loginUserActionCreator = loginUserActionCreator ?? throw new ArgumentNullException(nameof(loginUserActionCreator));
            this.externalServiceLoginUserActionCreator = facebookLoginUserActionCreator ?? throw new ArgumentNullException(nameof(facebookLoginUserActionCreator));

            var loginSubscription = appStore
                .DistinctUntilChanged(state => new { state.UserState.Login.Status })
                .SubscribeWithError(state =>
                {
                    var loginStatus = state.UserState.Login.Status;

                    if (loginStatus == Status.Setting)
                    {
                        IsBusy = true;
                        IsVisibleWrongCredentialLabel = false;
                    }
                    else if (loginStatus == Status.Error)
                    {
                        IsBusy = false;
                        IsVisibleWrongCredentialLabel = true;
                    }
                    else if(loginStatus == Status.Success)
                    {
                        IsBusy = false;
                        App.Current.MainPage.Navigation.PopModalAsync();
                    }

                }, error => { appStore.Dispatch(new SetErrorAction(error, "LoginViewModel - subscription.")); });
            subscriptions.Add(loginSubscription);
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

        private bool isWebViewVisible;
        public bool IsWebViewVisible
        {
            get => isWebViewVisible;
            set
            {
                isWebViewVisible = value;
                OnPropertyChanged();
            }
        }

        public WebView WebView {get; set;}

        public ICommand LoginCommand => new Command(Login);
        public ICommand LoginWithFacebookCommand => new Command(LoginWithFacebook);
        public ICommand LoginWithGoogleCommand => new Command(LoginWithGoogle);
        public ICommand SkipLoginCommand => new Command(SkipLogin);
        public ICommand RegisterCommand => new Command(Register);

        private void Login()
        {
            LoginWith = AccessProvider.SpotsFinderService;
            appStore.DispatchAsync(loginUserActionCreator.Login(username, password));   
        }

        private async void SkipLogin()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void Register()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new RegisterUserPage());
        }

        private void LoginWithFacebook()
        {
            if(WebView != null)
            {
                LoginWith = AccessProvider.Facebook;
                IsWebViewVisible = true;
                WebView.Source = FacebookLoginUrl;
                WebView.Navigated += WebView_Navigated;
            }
        }

        private void LoginWithGoogle()
        {
            if (WebView != null)
            {
                LoginWith = AccessProvider.Google;
                IsWebViewVisible = true;
                WebView.Source = GoogleLoginUrl;
                WebView.Navigated += WebView_Navigated;
            }
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            var redirectUrl = e.Url;
            if (!redirectUrl.Contains("access_token=") && !redirectUrl.Contains("?code="))
                return;

            WebView.Navigated -= WebView_Navigated;

            IsWebViewVisible = false;
            IsBusy = false;

            appStore.DispatchAsync(externalServiceLoginUserActionCreator.Login(redirectUrl, LoginWith));
        }
    }
}
