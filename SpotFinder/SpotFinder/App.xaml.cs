using Redux;
using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Services;
using SpotFinder.SQLite;
using SpotFinder.Views.Root;
using System.Reactive.Linq;
using System;
using Xamarin.Forms;
using SpotFinder.Redux.Actions;
using SpotFinder.Models.Core;
using SpotFinder.Core.Enums;
using System.Collections.Generic;

namespace SpotFinder
{
    public partial class App : Application
    {
        public static Store<ApplicationState> AppStore { get; private set; }

        private IPermissionHelper permissionHelper;
        private ISettingsHelper settingsHelper;
        private IDeviceLocationProvider deviceLocationProvider;
        private IPlaceManager placeManager;

        public App()
        {
            permissionHelper = DIContainer.Instance.Resolve<IPermissionHelper>();
            settingsHelper = DIContainer.Instance.Resolve<ISettingsHelper>();
            deviceLocationProvider = DIContainer.Instance.Resolve<IDeviceLocationProvider>();
            placeManager = DIContainer.Instance.Resolve<IPlaceManager>();

            InitializeComponent();

            permissionHelper.CheckAllPermissionAsync();

            AppStore = new Store<ApplicationState>(
                DIContainer.Instance.Resolve<IReducer<ApplicationState>>().Reduce,
                DIContainer.Instance.Resolve<ApplicationState>()
            );

            //Initial:
            AppStore.Dispatch(new ReadSettingsAction(settingsHelper.ReadSettings()));
            AppStore.Dispatch(new RequestGettingDeviceLocationLoop());
   
            MainPage = new CustomNavigationPage(new RootMasterDetailPage());

            GetDefaultSpots();
            SaveSettingsSubscription();
            DownloadPlacesByCriteriaSubscription();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void GetDefaultSpots()
        {
            //To jest potrzebne do ściągnięcia ReportManagerem listy miejsc, który po ściągnięciu ich ustawi Listę w stanie.
            AppStore.Dispatch(new SetInitialCriteriaAction(new Criteria
            {
                Location = new CityLocation
                {
                    City = AppStore.GetState().Settings.MainCity
                },
                Distance = AppStore.GetState().Settings.MainDistance,
                Type = new List<PlaceType> { PlaceType.Skatepark, PlaceType.Skatespot, PlaceType.DIY }
            }));
        }

        private void SaveSettingsSubscription()
        {
            //Subsktrybcja zapisująca ustawienia:
            AppStore
                .DistinctUntilChanged(state => new { state.Settings })
                .Subscribe(state =>
                {
                    AppStore.Dispatch(new SaveSettingsAction(state.Settings));
                });
        }

        private void DownloadPlacesByCriteriaSubscription()
        {
            //Subskrybcja, która dzięki zmianie Criteria w stanie powoduje PlaceManagera pobranie listy miejsc
            //Pobieranie listy:
            //1. Należy wywołać App.AppStore.Dispatch(new ReplaceCriteriaAction(criteria)); co wywoła poniższą subskrybcję, która następnie ustawi Criteria
            //   w PlaceManagera, a on zajmie się pobraniem listy, a gdy skończy wywoła  App.AppStore.Dispatch(new ListOfPlacesAction(DownloadedPlacesList));
            AppStore.
                DistinctUntilChanged(state => new { state.PlacesData.Criteria })
                .Subscribe(state =>
                {
                    if (state.PlacesData.ListOfPlaces == null)
                    {
                        placeManager.DownloadPlacesByCriteriaAsync(state.PlacesData.Criteria);
                    }
                });
        }
    }
}
