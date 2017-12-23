using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using SpotFinder.Redux.Actions;
using SpotFinder.Repositories;
using SpotFinder.DataServices;
using System;

namespace SpotFinder.ViewModels
{
    public class LocateOnMapViewModel : BaseViewModel
    {
        private IPlaceService PlaceService { get; }
        private ILocalPlaceRepository LocalPlaceRepository { get; }

        public LocateOnMapViewModel(IPlaceService placeService, ILocalPlaceRepository localPlaceRepository)
        {
            PlaceService = placeService ?? throw new ArgumentNullException("PlaceService is null in LocateOnMapViewModel");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("LocalPlaceRepository is null in LocateOnMapViewModel");

            var report = App.AppStore.GetState().PlacesData.Report;

            if(report != null && report.Location != null)
            {
                mapCenterLocation = new Position(report.Location.Latitude, report.Location.Longitude);
            }
            else
            {
                mapCenterLocation = new Position(19, 51);
                Device.BeginInvokeOnMainThread(() => 
                {
                    App.Current.MainPage.DisplayAlert("Ehh", "Problem with device location", "Ok :(");
                });
            }

            var mapTypeFromSettings = App.AppStore.GetState().Settings.MapType;

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

        private Position mapCenterLocation;
        public Position MapCenterLocation
        {
            get => mapCenterLocation;
            set
            {
                mapCenterLocation = value;
                OnPropertyChanged();
            }
        }

        public ICommand UploadSpotCommand => new Command(Upload);

        private async void Upload()
        {
            //Póki co nie wyrzkostuję lokalizacji z tej strony...
            IsBusy = true;
            App.AppStore.Dispatch(new PassLocationToReportingPlaceAction());
            var addingPlace = App.AppStore.GetState().PlacesData.Report.Place;

            int result = await PlaceService.SendAsync(addingPlace);

            if (result == 0)
            {
                var alertResult = await App.Current.MainPage.DisplayAlert(
                    "Error.", 
                    "Problem with publishing spot. Try again or add to offline list", 
                    "Try again", "Add local"
                );

                if (alertResult == true)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    LocalPlaceRepository.InsertPlace(addingPlace);
                }
            }

            IsBusy = false;

            await App.Current.MainPage.Navigation.PopToRootAsync();

            Device.BeginInvokeOnMainThread(() =>
            {
                App.Current.MainPage.DisplayAlert(
                    "Sukces!", 
                    "Miejsce zostało wysłane na serwer. Dziękujemy za dodanie nowego miejsca.", 
                    "Ok"
                );
            });
        }
    }
}
