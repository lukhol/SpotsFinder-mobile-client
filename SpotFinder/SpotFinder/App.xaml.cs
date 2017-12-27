using Redux;
using SpotFinder.Config;
using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Views.Root;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace SpotFinder
{
    public partial class App : Application
    {
        private IStore<ApplicationState> AppStore { get; }

        private IBootstrapper bootstrapper;
        private ISettingsHelper settingsHelper;

        public App()
        {
            AppStore = DIContainer.Instance.Resolve<IStore<ApplicationState>>();

            bootstrapper = DIContainer.Instance.Resolve<IBootstrapper>();
            settingsHelper = DIContainer.Instance.Resolve<ISettingsHelper>();

            InitializeComponent();

            AppStore.Dispatch(new ReadSettingsAction(settingsHelper.ReadSettings()));
   
            MainPage = new CustomNavigationPage(new RootMasterDetailPage());

            SaveSettingsSubscription();
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
