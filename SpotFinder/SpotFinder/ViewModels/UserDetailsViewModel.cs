using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Redux;
using SpotFinder.DataServices;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Redux.StateModels;
using SpotFinder.Services;
using SpotFinder.Views;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class UserDetailsViewModel : BaseViewModel
    {
        private readonly IPhotoProvider photoProvider;
        private readonly IUserService userService;

        public UserDetailsViewModel(IStore<ApplicationState> appStore, IPhotoProvider photoProvider,
            IUserService userService) : base(appStore)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(photoProvider));
            this.photoProvider = photoProvider ?? throw new ArgumentNullException(nameof(photoProvider));

            var userDetailsSubscription = appStore
                .DistinctUntilChanged(state => new { state.UserState.User })
                .SubscribeWithError(state =>
                {
                    var user = state.UserState.User;
                    if (user == null)
                    {
                        IsBusy = true;
                        return;
                    }

                    IsBusy = false;
                    SetViewFields(user);
                },
                error => { appStore.Dispatch(new SetErrorAction(error, "UserDetailsSubscription in UserDetailsViewModel.")); });
            subscriptions.Add(userDetailsSubscription);
        }

        private string firstname;
        public string Firstname
        {
            get => firstname;
            set
            {
                firstname = value;
                OnPropertyChanged();
            }
        }

        private string lastname;
        public string Lastname
        {
            get => lastname;
            set
            {
                lastname = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string avatarUrl;
        public string AvatarUrl
        {
            get => avatarUrl;
            set
            {
                avatarUrl = value;
                OnPropertyChanged();
            }
        }

        private string pageTitle;
        public string PageTitle
        {
            get => pageTitle;
            set
            {
                pageTitle = value;
                OnPropertyChanged();
            }
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

        public CachedImage AvatarFFCachedImage { get; set; }

        public ICommand LoginUserCommand => new Command(LoginUser);
        public ICommand LogoutUserCommand => new Command(LogoutUser);
        public ICommand ChangeAvatarCommand => new Command(ChangeAvatar);

        private void SetViewFields(User user)
        {
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            Email = user.Email;
            AvatarUrl = user.AvatarUrl;
            PageTitle = string.Format("{0} {1}", firstname, lastname);
        }

        private void LoginUser()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
        }

        private void LogoutUser()
        {
            appStore.Dispatch(new SetLoggedInUserAction(null));
        }

        private async void ChangeAvatar()
        {
            IsImageBusy = true;

            var avatarStream = await photoProvider.GetPhotoAsStreamAsync(Core.Enums.GetPhotoType.Camera);
            if (avatarStream == null)
            {
                IsImageBusy = false;
                return;
            }

            var newAvatarUrl = await SetAvatarOnServerAndUserInAppStore(avatarStream);
            UpdateAvatarView(newAvatarUrl);
        }

        private async Task<string> SetAvatarOnServerAndUserInAppStore(Stream avatarStream)
        {
            var user = appStore.GetState().UserState.User;
            var newAvatarUrl = await userService.SetAvatarAsync(user.Id, avatarStream);
            appStore.Dispatch(new SetUserAvatarUrlAction(newAvatarUrl));
            return newAvatarUrl;
        }

        private async void UpdateAvatarView(string newAvatarUrl)
        {
            await CachedImage.InvalidateCache(newAvatarUrl, CacheType.All, true);
            AvatarUrl = newAvatarUrl;
            AvatarFFCachedImage.Success += (s, e) => IsImageBusy = false;
        }
    }
}
