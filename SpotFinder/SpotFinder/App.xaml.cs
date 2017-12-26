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

        public App()
        {
            bootstrapper = DIContainer.Instance.Resolve<IBootstrapper>();
            settingsHelper = DIContainer.Instance.Resolve<ISettingsHelper>();

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
    }
}
