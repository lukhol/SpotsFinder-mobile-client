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
using SpotFinder.Redux.StateModels;
using SpotFinder.Config;

namespace SpotFinder
{
    public partial class App : Application
    {
        public static IStore<ApplicationState> AppStore { get; private set; }

        private IBootstrapper bootstrapper;

        private ISettingsHelper settingsHelper;
        private IPlaceManager placeManager;

        public App()
        {
            bootstrapper = DIContainer.Instance.Resolve<IBootstrapper>();

            settingsHelper = DIContainer.Instance.Resolve<ISettingsHelper>();
            placeManager = DIContainer.Instance.Resolve<IPlaceManager>();

            InitializeComponent();

            AppStore = new Store<ApplicationState>(
                DIContainer.Instance.Resolve<IReducer<ApplicationState>>().Reduce,
                DIContainer.Instance.Resolve<ApplicationState>()
            );

            AppStore.Dispatch(new ReadSettingsAction(settingsHelper.ReadSettings()));
   
            MainPage = new CustomNavigationPage(new RootMasterDetailPage());

            SaveSettingsSubscription();
            //DownloadPlacesByCriteriaSubscription();
        }

        protected override void OnStart()
        {
            bootstrapper.OnStart();
        }

        protected override void OnSleep()
        {
            bootstrapper.OnSleep();
        }

        protected override void OnResume()
        {
            bootstrapper.OnResume();
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

        //private void DownloadPlacesByCriteriaSubscription()
        //{
        //    //Subskrybcja, która dzięki zmianie Criteria w stanie powoduje PlaceManagera pobranie listy miejsc
        //    //Pobieranie listy:
        //    //1. Należy wywołać App.AppStore.Dispatch(new ReplaceCriteriaAction(criteria)); co wywoła poniższą subskrybcję, która następnie ustawi Criteria
        //    //   w PlaceManagera, a on zajmie się pobraniem listy, a gdy skończy wywoła  App.AppStore.Dispatch(new ListOfPlacesAction(DownloadedPlacesList));
        //    AppStore.
        //        DistinctUntilChanged(state => new { state.PlacesData.Criteria })
        //        .Subscribe(state =>
        //        {
        //            if (state.PlacesData.PlacesListState == null)
        //            {
        //                placeManager.DownloadPlacesByCriteriaAsync(state.PlacesData.Criteria);
        //            }
        //        });
        //}
    }
}
