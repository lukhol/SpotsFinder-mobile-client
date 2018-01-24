using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Redux;
using SpotFinder.DataServices;
using SpotFinder.Exceptions;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using SpotFinder.Services;
using SpotFinder.Views;
using System;
using System.Collections.Generic;
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
        private readonly IUpdateUserActionCreator updateUserActionCreator;
        private readonly URLRepository urlRepository;

        public UserDetailsViewModel(IStore<ApplicationState> appStore, IPhotoProvider photoProvider,
            IUserService userService, IUpdateUserActionCreator updateUserActionCreator, URLRepository urlRepository) : base(appStore)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(photoProvider));
            this.photoProvider = photoProvider ?? throw new ArgumentNullException(nameof(photoProvider));
            this.updateUserActionCreator = updateUserActionCreator ?? throw new ArgumentNullException(nameof(updateUserActionCreator));
            this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));

            var userDetailsSubscription = appStore
                .DistinctUntilChanged(state => new { state.UserState.User })
                .SubscribeWithError(state =>
                {
                    var user = state.UserState.User;
                    if (user == null)
                    {
                        IsUserLoggedIn = false;
                        return;
                    }

                    IsUserLoggedIn = true;
                    SetViewFields(user);
                },
                error => { appStore.Dispatch(new SetErrorAction(error, "UserDetailsSubscription in UserDetailsViewModel.")); });
            subscriptions.Add(userDetailsSubscription);

            var updateUserSubscription = appStore
                .DistinctUntilChanged(state => new { state.UserState.Edit })
                .Subscribe(state =>
                {
                    var edit = state.UserState.Edit;
                    if(edit.Status == Core.Enums.Status.Setting)
                    {
                        IsBusy = true;
                    }
                    if(edit.Status == Core.Enums.Status.Error)
                    {
                        IsBusy = false;
                        var error = edit.Error as EditUserException;
                        if (error != null)
                        {
                            IsEmailValid = false;
                            if (error.ServerErrorMessage != null)
                                EmailMessage = error.ServerErrorMessage;

                            if (error.EmailOccupidMessage != null)
                                EmailMessage = error.EmailOccupidMessage;
                        }
                    }
                    if(edit.Status == Core.Enums.Status.Success)
                    {
                        App.Current.MainPage.Navigation.PopAsync();
                    }
                });
            subscriptions.Add(updateUserSubscription);
        }

        private bool isUserLoggedIn;
        public bool IsUserLoggedIn
        {
            get => isUserLoggedIn;
            set
            {
                isUserLoggedIn = value;
                OnPropertyChanged();
            }
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

        private string emailMessage = "Email is invalid.";
        public string EmailMessage
        {
            get => emailMessage;
            set
            {
                emailMessage = value;
                OnPropertyChanged();
            }
        }

        private CachedImage avatarFFCachedImage;
        public CachedImage AvatarFFCachedImage
        {
            get => avatarFFCachedImage;
            set
            {
                avatarFFCachedImage = value;
                OnCachedImageAvatarChaged?.Invoke();
            }
        }

        event Action OnCachedImageAvatarChaged;

        public ICommand LoginUserCommand => new Command(LoginUser);
        public ICommand LogoutUserCommand => new Command(LogoutUser);
        public ICommand ChangeAvatarCommand => new Command(ChangeAvatar);
        public ICommand SaveChangesCommand => new Command(SaveChanges);
        public ICommand UserPlacesCommand => new Command(UserPlaces);

        private void SetViewFields(User user)
        {
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            Email = user.Email;
            PageTitle = string.Format("{0} {1}", firstname, lastname);

            if (user.AvatarUrl.Contains("http"))
                AvatarUrl = user.AvatarUrl;
            else
                AvatarUrl = urlRepository.BaseUrl + user.AvatarUrl;

            OnCachedImageAvatarChaged += () => 
            {
                avatarFFCachedImage.DownloadStarted += (s, e) =>
                {
                    IsImageBusy = true;
                };

                avatarFFCachedImage.Success += (s, e) =>
                {
                    IsImageBusy = false;
                };
            };
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
            await CachedImage.InvalidateCache(urlRepository.BaseUrl + newAvatarUrl, CacheType.All, true);
            await CachedImage.InvalidateCache(AvatarFFCachedImage.Source, CacheType.All, true);

            AvatarUrl = string.Empty;
            AvatarUrl = urlRepository.BaseUrl + newAvatarUrl;
        }

        private void SaveChanges()
        {
            if (!isEmailValid)
            {
                return;
            }

            IDictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("firstname", firstname);
            fields.Add("lastname", lastname);
            fields.Add("email", email);

            appStore.DispatchAsync(updateUserActionCreator.UpdateUser(fields));
        }

        private void UserPlaces()
        {

        }
    }
}
