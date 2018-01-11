using Redux;
using SpotFinder.Config;
using SpotFinder.Redux;
using SpotFinder.Views;
using SpotFinder.Views.Root;
using Xamarin.Forms;

namespace SpotFinder
{
    public partial class App : Application
    {
        private IBootstrapper bootstrapper;
        private IStore<ApplicationState> appStore;

        public App()
        {
            bootstrapper = DIContainer.Instance.Resolve<IBootstrapper>();
            appStore = DIContainer.Instance.Resolve<IStore<ApplicationState>>();

            InitializeComponent();

            MainPage = new CustomNavigationPage(new RootMasterDetailPage());
        }

        protected async override void OnStart()
        {
            bootstrapper.OnStart();

            if(appStore.GetState().UserState.User == null)
                await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
        }

        protected override void OnSleep()
        {
            bootstrapper.OnSleep();
        }

        protected override void OnResume()
        {
            bootstrapper.OnResume();
        }
    }
}
