using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Services;
using SpotFinder.SQLite;
using SpotFinder.Views.Root;
using Xamarin.Forms;

namespace SpotFinder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new RootMasterDetailPage())
            {
                BarBackgroundColor = Color.Green,
                BarTextColor = Color.White,
                BackgroundColor = Color.FromRgb(0, 0, 0)
            };

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
