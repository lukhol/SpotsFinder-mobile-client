using SpotFinder.Models.Core;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using Xamarin.Forms.Maps;
using System.Collections.ObjectModel;
using SpotFinder.Core;
using SpotFinder.Redux;
using Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Views;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.ViewModels
{
    public class PlaceDetailsViewModel : BaseViewModel
    {
        private Place place;
        private IDisposable placeSubscription;

        public PlaceDetailsViewModel(IStore<ApplicationState> appStore) : base(appStore)
        {
            IsBusy = true;

            placeSubscription = appStore
                .DistinctUntilChanged(state => new { state.PlacesData.CurrentPlaceState.Value })
                .Subscribe(state =>
                {
                    var place = state.PlacesData.CurrentPlaceState.Value;
                    if(place != null && state.PlacesData.CurrentPlaceState.Status == Core.Enums.Status.Success)
                    {
                        this.place = place;
                        Update();
                    }
                }, error => { appStore.Dispatch(new SetErrorAction(error, "PlaceDetailsViewModel - subscription.")); });

            var mapTypeFromSettings = appStore.GetState().Settings.MapType;

            switch (mapTypeFromSettings)
            {
                case Core.Enums.MapType.Normal:
                    mapTypeProperty = MapType.Street;
                    break;

                case Core.Enums.MapType.Satelite:
                    mapTypeProperty = MapType.Satellite;
                    break;

                default:
                    mapTypeProperty = MapType.Street;
                    break;
            }
        }

        private bool isEditPlaceButtonVisible;
        public bool IsEditPlaceButtonVisible
        {
            get => isEditPlaceButtonVisible;
            set
            {
                isEditPlaceButtonVisible = value;
                OnPropertyChanged();
            }
        }

        private MapType mapTypeProperty;
        public MapType MapTypeProperty
        {
            get => mapTypeProperty;
            set
            {
                mapTypeProperty = value;
                OnPropertyChanged();
            }
        }

        public Place Place
        {
            get => place;
            set
            {
                place = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Type");
                OnPropertyChanged("Description");
                OnPropertyChanged("ObstacleList");
                OnPropertyChanged("ObstacleListHeight");
                IsBusy = false;
            }
        }

        private Position spotPosition;
        public Position SpotPosition
        {
            get => spotPosition;
            set
            {
                spotPosition = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Pin> spotPin;
        public ObservableCollection<Pin> SpotPin
        {
            get => spotPin;
            set
            {
                spotPin = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                if (place != null)
                    return place.Name;
                else
                    return "Place = null.";
            }
        }

        public string Description
        {
            get
            {
                if (place != null)
                    return place.Description;
                else
                    return "Place = null.";
            }
        }

        public string Type
        {
            get
            {
                if (place != null)
                    return place.Type.ToString();
                else
                    return "Place = null.";
            }
        }

        public List<string> ObstacleList
        {
            get
            {
                return PrepareObstacleList();
            }
        }

        private ObservableCollection<View> imagesList;
        public ObservableCollection<View> ImagesList
        {
            get => imagesList;
            set
            {
                imagesList = value;
                OnPropertyChanged();
            }
        }

        public double ObstacleListHeight
        {
            get => PrepareObstacleList().Count * 20;
        }

        public override ICommand GoBackCommand => new Command(() =>
        {
            App.Current.MainPage.Navigation.PopAsync();
        });

        public ICommand ReportPlaceCommand => new Command(ReportPlace);
        public ICommand EditPlaceCommand => new Command(EditPlace);

        private List<string> PrepareObstacleList()
        {
            var list = new List<string>();

            if (place == null)
            {
                list.Add("Place is null. Pleace report this bug.");
                return list;
            }

            if (place.Stairs == true)
                list.Add("-Stairs");

            if (place.Rail == true)
                list.Add("-Rail");

            if (place.Ledge == true)
                list.Add("-Ledge");

            if (place.Handrail == true)
                list.Add("-Handrail");

            if (place.Hubba == true)
                list.Add("-Hubba");

            if (place.Corners == true)
                list.Add("-Corners");

            if (place.Manualpad == true)
                list.Add("-Manualpad");

            if (place.Wallride == true)
                list.Add("-Wallride");

            if (place.Downhill == true)
                list.Add("-Downhill");

            if (place.OpenYourMind == true)
                list.Add("-OpenYourMind");

            if (place.Pyramid == true)
                list.Add("-Pyramid");

            if (place.Curb == true)
                list.Add("-Curb");

            if (place.Bank == true)
                list.Add("-Bank");

            if (place.Bowl == true)
                list.Add("-Bowl");

            return list;
        }

        private void Update()
        {
            //Add spot as a pin to the map:
            SpotPosition = new Position(place.Location.Latitude, place.Location.Longitude);

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = spotPosition,
                Label = place.Name + " ",
                Address = place.Description + " "
            };

            var pinAsList = new ObservableCollection<Pin>();
            pinAsList.Add(pin);
            SpotPin = pinAsList;

            //Add photos:
            var newImagesList = new ObservableCollection<View>();
            foreach (var placeImage in place.PhotosBase64List)
            {
                var image = new Image();
                image.Aspect = Aspect.AspectFill;
                image.Source = Utils.Base64ImageToImageSource(placeImage);
                newImagesList.Add(image);
            }

            ImagesList = newImagesList;

            Place = this.place;

            IsEditPlaceButtonVisible = CheckIfPlaceBelongsToLoggedInUser(appStore.GetState().UserState.User);
            IsBusy = false;

            placeSubscription.Dispose();
        }

        private bool CheckIfPlaceBelongsToLoggedInUser(User user)
        {
            if (user != null)
            {
                if (place != null)
                {
                    if (place.UserId == user.Id)
                        return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        private void ReportPlace()
        {
            App.Current.MainPage.Navigation.PushAsync(new ReportPlacePage());
        }

        private void EditPlace()
        {
            appStore.Dispatch(new SetUpdateReportPlaceAction(place));
            App.Current.MainPage.Navigation.PushAsync(new AddingProcessPage());
        }
    }
}
