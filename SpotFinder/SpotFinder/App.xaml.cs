using Redux;
using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Services;
using SpotFinder.SQLite;
using System.Reactive.Linq;
using System;
using Xamarin.Forms;
using SpotFinder.Redux.Actions;
using SpotFinder.Views.Root.Xaml;

namespace SpotFinder
{
    public partial class App : Application
    {
        public static Store<ApplicationState> AppStore { get; private set; }

        private IPermissionHelper permissionHelper;
        private IDeviceLocationHelper deviceLocationHelper;
        private ISettingsHelper settingsHelper;

        public App()
        {
            permissionHelper = Unity.Instance.Resolve<IPermissionHelper>();
            deviceLocationHelper = Unity.Instance.Resolve<IDeviceLocationHelper>();
            settingsHelper = Unity.Instance.Resolve<ISettingsHelper>();

            InitializeComponent();
            
            AppStore = new Store<ApplicationState>(
                Unity.Instance.Resolve<IReducer<ApplicationState>>().Reduce, 
                Unity.Instance.Resolve<ApplicationState>()
            );
            
            MainPage = new CustomNavigationPage(RootMasterDetailPage());
            
            AppStore
                .DistinctUntilChanged(state => new { state.Settings })
                .Subscribe(state =>
                {
                    if (state.Settings == null)
                        AppStore.Dispatch(new ReadSettingsAction(settingsHelper.ReadSettings()));
                    else
                        AppStore.Dispatch(new SaveSettingsAction(state.Settings));
                });
            
            SQLiteConfig.Start();
            UnityConfig.Start();

            deviceLocationHelper.GetLocationAndSaveInReportManagerOnStartAsync();
            permissionHelper.CheckAllPermissionAsync();
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
    }
}
