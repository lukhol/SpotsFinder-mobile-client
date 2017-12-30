using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions.PlacesList;
using SpotFinder.Resx;
using SpotFinder.Views.Root;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class CriteriaViewModel : BaseViewModel
    {
        private IGetPlacesListByCriteriaActionCreator downloadPlacesListByCriteriaActionCreator;

        public CriteriaViewModel(IStore<ApplicationState> appStore, 
            IGetPlacesListByCriteriaActionCreator downloadPlacesListByCriteriaActionCreator) : base(appStore)
        {
            this.downloadPlacesListByCriteriaActionCreator = downloadPlacesListByCriteriaActionCreator ?? throw new ArgumentNullException(nameof(downloadPlacesListByCriteriaActionCreator));

            distance = 10;
            usePhoneLocation = true;
            skatepark = true;
            skatespot = true;
            diy = true;
        }

        private bool skatepark;
        public bool Skatepark
        {
            get => skatepark;
            set
            {
                skatepark = value;
                OnPropertyChanged();
            }
        }

        private bool skatespot;
        public bool Skatespot
        {
            get => skatespot;
            set
            {
                skatespot = value;
                OnPropertyChanged();
            }
        }

        private bool diy;
        public bool Diy
        {
            get => diy;
            set
            {
                diy = value;
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

        private double distance;
        public double Distance
        {
            get => distance;
            set
            {
                distance = value;
                distanceLabelText = AppResources.CriteriaDistanceLabel + ((int)distance).ToString() + " km";
                OnPropertyChanged();
                OnPropertyChanged("DistanceLabelText");
            }
        }

        private string distanceLabelText = AppResources.CriteriaDistanceLabel + "1 km";
        public string DistanceLabelText
        {
            get => distanceLabelText;
        }

        private bool usePhoneLocation;
        public bool UsePhoneLocation
        {
            get => usePhoneLocation;
            set
            {
                usePhoneLocation = value;
                useCity = !value;
                OnPropertyChanged();
                OnPropertyChanged("UseCity");
            }
        }


        private bool useCity;
        public bool UseCity
        {
            get => !usePhoneLocation;
            set
            {
                useCity = value;
                usePhoneLocation = !value;
                OnPropertyChanged();
                OnPropertyChanged("UsePhoneLocation");
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand => new Command(SearchRequest);

        private void SearchRequest()
        {
            var deviceLocation = appStore.GetState().DeviceData.LocationState.Value;

            if (deviceLocation == null)
                return;

            //Types:
            var listOfTypes = new List<PlaceType>();

            if (skatespot)
                listOfTypes.Add(PlaceType.Skatepark);

            if (skatepark)
                listOfTypes.Add(PlaceType.Skatespot);

            if (diy)
                listOfTypes.Add(PlaceType.DIY);

            CityLocation cityLocation = null;
            if (usePhoneLocation)
            {
                cityLocation = new CityLocation
                {
                    Longitude = deviceLocation.Longitude,
                    Latitude = deviceLocation.Latitude
                };
            }
            else
            {
                if (!string.IsNullOrEmpty(city))
                {
                    cityLocation = new CityLocation
                    {
                        City = city,
                        Latitude = null,
                        Longitude = null
                    };
                }
            }

            var criteria = new Criteria(listOfTypes, cityLocation, (int)distance, gap, stairs, rail, ledge, handrail, corners,
                manualpad, wallride, downhill, openYourMind, pyramid, curb, bank, bowl, hubba);

            //Request pobiernaia
            appStore.DispatchAsync(downloadPlacesListByCriteriaActionCreator.DownloadPlaceByCriteria(criteria));

            App.Current.MainPage.Navigation.PopAsync();

            Device.BeginInvokeOnMainThread(() =>
            {
                if(App.Current.MainPage.Navigation.NavigationStack.Last().GetType() == typeof(RootMasterDetailPage))
                    App.Current.MainPage.DisplayAlert("Wyszukiwanie rozpoczęte!", "Wyszukiwanie zostało rozpoczęte. W celu sprawdzenia rezultatów przejdz do mapy lub listy.", "Ok");
            });
        }
    }
}
