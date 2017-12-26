using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Services;
using SpotFinder.Views.Root;
using System.Reactive.Linq;
using System;
using Xamarin.Forms;
using SpotFinder.Redux.Actions;
using SpotFinder.Models.Core;
using SpotFinder.Core.Enums;
using System.Collections.Generic;
using Redux;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.Actions.Locations;

namespace SpotFinder
{
    public partial class App : Application
    {
        public static IStore<ApplicationState> AppStore { get; private set; }

        private ISettingsHelper settingsHelper;
        private IPlaceManager placeManager;

        private IPermissionActionCreator permissionActionCreator;
        private IDeviceLocationActionCreator deviceLocationActionCreator;

        public App()
        {
            settingsHelper = DIContainer.Instance.Resolve<ISettingsHelper>();
            placeManager = DIContainer.Instance.Resolve<IPlaceManager>();
            permissionActionCreator = DIContainer.Instance.Resolve<IPermissionActionCreator>();
            deviceLocationActionCreator = DIContainer.Instance.Resolve<IDeviceLocationActionCreator>();

            InitializeComponent();

            AppStore = new Store<ApplicationState>(
                DIContainer.Instance.Resolve<IReducer<ApplicationState>>().Reduce,
                DIContainer.Instance.Resolve<ApplicationState>()
            );

            AppStore.DispatchAsync(permissionActionCreator.CheckPermissions(
                PermissionName.Location,
                PermissionName.Storage,
                PermissionName.Camera)
            );

            //Initial:
            AppStore.Dispatch(new ReadSettingsAction(settingsHelper.ReadSettings()));
   
            MainPage = new CustomNavigationPage(new RootMasterDetailPage());

            DeviceLocationSubscription();
            GetDefaultSpots();
            SaveSettingsSubscription();
            DownloadPlacesByCriteriaSubscription();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void DeviceLocationSubscription()
        {
            AppStore
                .DistinctUntilChanged(state => new { state.DeviceData.LocationState.Status })
                .Subscribe(state =>
                {
                    var locationStatus = state.DeviceData.LocationState.Status;
                    if (locationStatus == Status.NotStartedYet || locationStatus == Status.Unknown)
                        AppStore.DispatchAsync(deviceLocationActionCreator.RequestDeviceLocation(TimeSpan.FromSeconds(8)));
                });
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
