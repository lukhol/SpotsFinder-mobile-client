﻿using Redux;
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
        private readonly string FacebookAppId;
        private readonly ILoginUserActionCreator loginUserActionCreator;
        private readonly IExternalServiceLoginUserActionCreator facebookLoginUserActionCreator;

        public LoginViewModel(
            IStore<ApplicationState> appStore, 
            string facebookAppId, 
            string facebookLoginUrl,
            IExternalServiceLoginUserActionCreator facebookLoginUserActionCreator,
            ILoginUserActionCreator loginUserActionCreator
            ) : base(appStore)
        {
            FacebookAppId = facebookAppId;
            FacebookLoginUrl = facebookLoginUrl;
            this.loginUserActionCreator = loginUserActionCreator ?? throw new ArgumentNullException(nameof(loginUserActionCreator));
            this.facebookLoginUserActionCreator = facebookLoginUserActionCreator ?? throw new ArgumentNullException(nameof(facebookLoginUserActionCreator));

            var loginSubscription = appStore
                .DistinctUntilChanged(state => state.UserState.Login.Status)
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
        public ICommand SkipLoginCommand => new Command(SkipLogin);
        public ICommand RegisterCommand => new Command(Register);

        private void Login()
        {
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
                IsWebViewVisible = true;
                WebView.Source = FacebookLoginUrl;
                WebView.Navigated += WebView_Navigated;
            }
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            var url = e.Url;
            if (url.Contains(FacebookAppId) || string.IsNullOrEmpty(url) || url.Contains("about:blank"))
                return;

            WebView.Navigated -= WebView_Navigated;

            IsWebViewVisible = false;
            IsBusy = false;
            appStore.DispatchAsync(facebookLoginUserActionCreator.Login(url));
        }
    }
}
