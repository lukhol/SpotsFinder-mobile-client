using SpotFinder.Config;
using SpotFinder.Views;
using SpotFinder.Views.Root;
using Xamarin.Forms;

namespace SpotFinder
{
    public partial class App : Application
    {
        private IBootstrapper bootstrapper;

        public App()
        {
            bootstrapper = DIContainer.Instance.Resolve<IBootstrapper>();
            InitializeComponent();
            MainPage = new CustomNavigationPage(new RootMasterDetailPage());
            App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
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
    }
}
