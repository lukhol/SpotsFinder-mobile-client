using Redux;
using SpotFinder.Config;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Views;
using SpotFinder.Views.Root;
using System.Threading.Tasks;
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

        protected override void OnStart()
        {
            bootstrapper.OnStart();
            LoginUser();
        }

        protected override void OnSleep()
        {
            bootstrapper.OnSleep();
        }

        protected override void OnResume()
        {
            bootstrapper.OnResume();
        }

        private async void LoginUser()
        {
            if (appStore.GetState().UserState.User == null)
                await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
            else
                await CheckAccessToken();
        }

        private async Task CheckAccessToken()
        {
            var userAccessActionCreator = DIContainer.Instance.Resolve<IUserAccessActionCreator>();
            await appStore.DispatchAsync(userAccessActionCreator.CheckTokens());
        }
    }
}
