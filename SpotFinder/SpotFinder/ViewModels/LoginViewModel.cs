﻿using Redux;
using SpotFinder.Redux;
using SpotFinder.Views.Root;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        //TO DO: PUT IT NOT THERE
        private static string FacebookAppId = "204040756811030";

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


        private async void Login()
        {
            if (username.Equals("admin") && username.Equals("admin"))
            {
                IsBusy = true;
                await Task.Delay(5000);
                IsBusy = false;
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
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

        private void LoginWithFacebook()
        {
            if(WebView != null)
            {
                IsWebViewVisible = true;
                WebView.Source = "https://www.facebook.com/dialog/oauth?client_id=204040756811030&response_type=token&redirect_uri=https://www.facebook.com/connect/login_success.html";
                WebView.Navigated += WebView_Navigated;
            }
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            var url = e.Url;
            if (url.Contains(FacebookAppId))
                return;

            //Logic here:

            IsWebViewVisible = false;
        }
    }
}
