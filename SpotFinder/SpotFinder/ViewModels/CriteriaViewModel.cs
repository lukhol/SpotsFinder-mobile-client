using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.Redux.Actions;
using SpotFinder.Resx;
using System.Linq;
using SpotFinder.Views.Root;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class CriteriaViewModel : BaseViewModel
    {
        public CriteriaViewModel()
        {
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

        private async void SearchRequest()
        {
            var deviceLocation = App.AppStore.GetState().DeviceData.LocationState.Value;

            if (deviceLocation == null)
                return;

            var criteria = new Criteria
            {
                Pyramid = pyramid,
                Hubba = hubba,
                Wallride = wallride,
                Gap = gap,
                Bank = bank,
                Bowl = bowl,
                Corners = corners,
                Curb = curb,
                Downhill = downhill,
                Handrail = handrail,
                Ledge = ledge,
                Manualpad = manualpad,
                Stairs = stairs,
                Rail = rail,
                OpenYourMind = openYourMind,
                Distance = (int)distance
            };

            //Types:
            var listOfTypes = new List<PlaceType>();

            if (skatespot)
                listOfTypes.Add(PlaceType.Skatepark);

            if (skatepark)
                listOfTypes.Add(PlaceType.Skatespot);

            if (diy)
                listOfTypes.Add(PlaceType.DIY);

            criteria.Type = listOfTypes;

            if (usePhoneLocation)
            {
                criteria.Location = new CityLocation
                {
                    Longitude = deviceLocation.Longitude,
                    Latitude = deviceLocation.Latitude
                };
            }
            else
            {
                if (!string.IsNullOrEmpty(city))
                {
                    criteria.Location = new CityLocation
                    {
                        City = city,
                        Latitude = null,
                        Longitude = null
                    };
                }
            }

            //Wyzeruj spot
            App.AppStore.Dispatch(new ClearSpotsListAction());
            //Request pobiernaia
            App.AppStore.Dispatch(new ReplaceCriteriaAction(criteria));

            await App.Current.MainPage.Navigation.PopAsync();

            Device.BeginInvokeOnMainThread(() =>
            {
                if(App.Current.MainPage.Navigation.NavigationStack.Last().GetType() == typeof(RootMasterDetailPage))
                    App.Current.MainPage.DisplayAlert("Wyszukiwanie rozpoczęte!", "Wyszukiwanie zostało rozpoczęte. W celu sprawdzenia rezultatów przejdz do mapy lub listy.", "Ok");
            });
        }
    }
}
