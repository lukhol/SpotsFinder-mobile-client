using Redux;
using SpotFinder.DataServices;
using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Repositories;
using SpotFinder.SQLite;
using SpotFinder.Views.Root;
using Xamarin.Forms;

namespace SpotFinder
{
    public partial class App : Application
    {
        public static Store<ApplicationState> AppStore { get; private set; }

        public App()
        {
            InitializeComponent();

            AppStore = new Store<ApplicationState>(
                ApplicationReducer.Reducer, 
                new ApplicationState(
                    new PlaceService(),
                    new LocalPlaceRepository()
                )
            );

            AppStore.Dispatch(new InitAction());

            MainPage = new CustomNavigationPage(new RootMasterDetailPageTwo());

            PermissionHelper.CheckAllPermissionAsync();
            SQLiteConfig.Start();
            UnityConfig.Start();
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
