using SpotFinder.Core;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.OwnControls;
using SpotFinder.Resx;
using SpotFinder.Services;
using System;
using System.Collections.Generic;
using SpotFinder.Redux.Actions;
using System.Windows.Input;
using Xamarin.Forms;
using SpotFinder.Views;
using System.Collections.ObjectModel;
using Redux;
using SpotFinder.Redux;
using System.Reactive.Linq;
using SpotFinder.Redux.StateModels;
using System.IO;

namespace SpotFinder.ViewModels
{
    public class AddingProcessViewModel : BaseViewModel
    {
        private IPhotoProvider PhotoProvider;

        public const int MAX_DESCRIPTION_LENGTH = 255;
        public const int MIN_DESCRIPTION_LENGTH = 5;

        public const int MAX_TITLE_LENGTH = 30;
        public const int MIN_TITLE_LENGTH = 3;

        public AddingProcessViewModel(IStore<ApplicationState> appStore, IPhotoProvider photoProvider) : base(appStore)
        {
            PhotoProvider = photoProvider ?? throw new ArgumentNullException("PhotoProvider is null in AddingProcessViewModel.");

            isNextButtonVisible = false;
            isPhotoButtonVisible = true;

            placeTypePickerItemList = new List<string>
            {
                "Skatepark", "Skatespot", "DIY"
            };

            var userSubscription = appStore
                .DistinctUntilChanged(state => new { state.UserState.User })
                .SubscribeWithError(state =>
                {
                    var user = state.UserState.User;
                    CheckIfUserIsLoggedIn(user);
                }, error => { });

            if (appStore.GetState().PlacesData.ReportState.Value?.ReportType == ReportType.Update)
                BindUpdatingPlaceToView();
        }

        private void CheckIfUserIsLoggedIn(User user)
        {
            IsBusy = (user == null) ? true : false;
        }

        private void BindUpdatingPlaceToView()
        {
            var place = appStore.GetState().PlacesData.ReportState.Value?.Place;
            if (place == null)
                return;

            Description = place.Description;
            Name = place.Name;
            SelectedTypeString = place.Type.ToString();

            Gap = place.Gap;
            Stairs = place.Stairs;
            Rail = place.Rail;
            Ledge = place.Ledge;
            Handrail = place.Handrail;
            Hubba = place.Hubba;
            Corners = place.Corners;
            Manualpad = place.Manualpad;
            Wallride = place.Wallride;
            Downhill = place.Downhill;
            OpenYourMind = place.OpenYourMind;
            Pyramid = place.Pyramid;
            Curb = place.Curb;
            Bank = place.Bank;
            Bowl = place.Bowl;

            foreach(var base64Image in place.PhotosBase64List)
            {
                MyImage myImage = new MyImage();
                myImage.Base64Representation = base64Image;

                myImage.Source = ImageSource.FromStream(() =>
                {
                    byte[] imageAsByte = Convert.FromBase64String(base64Image);
                    var memoryStream = new MemoryStream(imageAsByte, 0, imageAsByte.Length);
                    return memoryStream;
                });

                PrepareImageToDisplay(myImage);
                ImagesList = UpdateImagesList(myImage);
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private List<string> placeTypePickerItemList;
        public List<string> PlaceTypePickerItemList
        {
            get => placeTypePickerItemList;
            set
            {
                placeTypePickerItemList = value;
                OnPropertyChanged();
            }
        }

        private string selectedTypeString;
        public string SelectedTypeString
        {
            get => selectedTypeString;
            set
            {
                selectedTypeString = value;
                OnPropertyChanged();
            }
        }

        //Obstacles: ========================================
        private bool gap;
        public bool Gap
        {
            get => gap;
            set
            {
                gap = value;
                OnPropertyChanged();
            }
        }

        private bool stairs;
        public bool Stairs
        {
            get => stairs;
            set
            {
                stairs = value;
                OnPropertyChanged();
            }
        }

        private bool rail;
        public bool Rail
        {
            get => rail;
            set
            {
                rail = value;
                OnPropertyChanged();
            }
        }

        private bool ledge;
        public bool Ledge
        {
            get => ledge;
            set
            {
                ledge = value;
                OnPropertyChanged();
            }
        }

        private bool handrail;
        public bool Handrail
        {
            get => handrail;
            set
            {
                handrail = value;
                OnPropertyChanged();
            }
        }

        private bool hubba;
        public bool Hubba
        {
            get => hubba;
            set
            {
                hubba = value;
                OnPropertyChanged();
            }
        }

        private bool corners;
        public bool Corners
        {
            get => corners;
            set
            {
                corners = value;
                OnPropertyChanged();
            }
        }

        private bool manualpad;
        public bool Manualpad
        {
            get => manualpad;
            set
            {
                manualpad = value;
                OnPropertyChanged();
            }
        }

        private bool wallride;
        public bool Wallride
        {
            get => wallride;
            set
            {
                wallride = value;
                OnPropertyChanged();
            }
        }

        private bool downhill;
        public bool Downhill
        {
            get => downhill;
            set
            {
                downhill = value;
                OnPropertyChanged();
            }
        }

        private bool openYourMind;
        public bool OpenYourMind
        {
            get => openYourMind;
            set
            {
                openYourMind = value;
                OnPropertyChanged();
            }
        }

        private bool pyramid;
        public bool Pyramid
        {
            get => pyramid;
            set
            {
                pyramid = value;
                OnPropertyChanged();
            }
        }

        private bool curb;
        public bool Curb
        {
            get => curb;
            set
            {
                curb = value;
                OnPropertyChanged();
            }
        }

        private bool bank;
        public bool Bank
        {
            get => bank;
            set
            {
                bank = value;
                OnPropertyChanged();
            }
        }

        private bool bowl;
        public bool Bowl
        {
            get => bowl;
            set
            {
                bowl = value;
                OnPropertyChanged();
            }
        }

        private bool isPhotoButtonVisible;
        public bool IsPhotoButtonVisible
        {
            get => isPhotoButtonVisible;
            set
            {
                isPhotoButtonVisible = value;
                OnPropertyChanged();
            }
        }

        private bool isNextButtonVisible;
        public bool IsNextButtonVisible
        {
            get => isNextButtonVisible;
            set
            {
                isNextButtonVisible = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<View> imagesList = new ObservableCollection<View>();
        public ObservableCollection<View> ImagesList
        {
            get => imagesList;
            set
            {
                imagesList = value;
                IsNextButtonVisible = imagesList.Count >= 1 ? true : false;
                IsPhotoButtonVisible = imagesList.Count >= 5 ? false : true;
                OnPropertyChanged();
            }
        }

        public ICommand LoginUserCommand => new Command(LoginUser);
        public ICommand ReportCommand => new Command(ReportFun);
        public ICommand AddPhotoCommand => new Command(AddPhotoAsync);
        
        private void LoginUser()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
        }

        private async void AddPhotoAsync()
        {
            var pickTypeResult = await App.Current.MainPage.DisplayAlert("Where?", "Chose from where you want pick photo:", "Camera", "Gallery");
            MyImage image;

            if (pickTypeResult)
                image = await PhotoProvider.GetPhotoAsync(GetPhotoType.Camera);
            else
                image = await PhotoProvider.GetPhotoAsync(GetPhotoType.Gallery);

            if (image == null)
                return;

            PrepareImageToDisplay(image);
            ImagesList = UpdateImagesList(image);
        }

        private void PrepareImageToDisplay(MyImage image)
        {
            var removeImageFromLayoutGestRecognizer = new TapGestureRecognizer();
            removeImageFromLayoutGestRecognizer.Tapped += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await App.Current.MainPage.DisplayAlert("Alert!", AppResources.AlertRemovePhoto, AppResources.AlertYes, AppResources.AlertNo);
                    if (result)
                    {
                        ImagesList = RemoveFromImagesList(image);
                        image.RemoveFromParent();
                    }
                }); 
            };
            image.GestureRecognizers.Add(removeImageFromLayoutGestRecognizer);
        }

        private ObservableCollection<View> UpdateImagesList(MyImage image)
        {
            var newImagesList = new ObservableCollection<View>(imagesList);
            newImagesList.Add(image);
            return newImagesList;
        }

        private ObservableCollection<View> RemoveFromImagesList(MyImage image)
        {
            var newImagesList = new ObservableCollection<View>(imagesList);
            newImagesList.Remove(image);
            return newImagesList;
        }

        private void ReportFun()
        {
            if (!ValidateFieldsBeforeGo())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage.DisplayAlert("UPS", "Walidacja", "Ok");
                });
                return;
            }

            var place = new Place
            {
                Bank = bank,
                Ledge = ledge,
                Handrail = handrail,
                Hubba = hubba,
                Bowl = bowl,
                Gap = gap,
                Stairs = stairs,
                Downhill = downhill,
                OpenYourMind = openYourMind,
                Corners = corners,
                Curb = curb, 
                Rail = rail,
                Pyramid = pyramid,
                Wallride = wallride,
                Manualpad = manualpad
            };

            switch(selectedTypeString)
            {
                case "Skatepark":
                    place.Type = PlaceType.Skatepark;
                    break;

                case "Skatespot":
                    place.Type = PlaceType.Skatespot;
                    break;

                case "DIY":
                    place.Type = PlaceType.DIY;
                    break;
            }

            place.Description = Utils.FirstLetterToUpperCase(description);
            place.Name = Utils.FirstLetterToUpperCase(name);

            var base64PhotosList = new List<string>();

            foreach(var myImageAsView in ImagesList)
            {
                var myImage = myImageAsView as MyImage;

                if (myImage == null)
                    continue;

                base64PhotosList.Add(myImage.Base64Representation);
            }

            place.PhotosBase64List = base64PhotosList;

            if(appStore.GetState().PlacesData.ReportState.Value.Place != null)
            {
                var placeFromState = appStore.GetState().PlacesData.ReportState.Value.Place;
                place.Id = placeFromState.Id;
                place.Version = placeFromState.Version;
                place.UserId = placeFromState.UserId;
                place.Location = placeFromState.Location;
            }

            appStore.Dispatch(new SetReportPlaceAction(place));
            App.Current.MainPage.Navigation.PushAsync(new LocateOnMapPage());
        }

        private bool ValidateFieldsBeforeGo()
        {
            if (name == null || description == null || imagesList == null || imagesList.Count == 0)
                return false;

            if (name.Length < MIN_TITLE_LENGTH || name.Length > MAX_TITLE_LENGTH)
                return false;

            if (description.Length < MIN_DESCRIPTION_LENGTH || description.Length > MAX_DESCRIPTION_LENGTH)
                return false;

            if (imagesList.Count <= 0 || imagesList.Count > 5)
                return false;

            return true;
        }
    }
}