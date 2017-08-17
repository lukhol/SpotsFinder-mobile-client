using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SpotFinder.Core;
using SpotFinder.Redux;
using SpotFinder.SQLite.Models;
using SQLite;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder
{
    public partial class App : Application
    {
        public static Store<State> Store;

        private readonly SQLiteConnection _sqLiteConnection;

        public App()
        {
            _sqLiteConnection = DependencyService.Get<ISQLite>().GetConnection();
            _sqLiteConnection.CreateTable<SQLitePlace>();

            InitializeComponent();

            MainPage = new NavigationPage(new Views.Root.RootMasterDetailPage())
            {
                BarBackgroundColor = Color.Green,
                BarTextColor = Color.White,
                //BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"]
                BackgroundColor = Color.FromRgb(0,0,0)
            };

            CheckPermissionsAsync();

            var unity = new UnityConfig(MainPage.Navigation);

            Store = new Store<State>((state, action) =>
            {
                return new State();
            }, new State());
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

        private async void CheckPermissionsAsync()
        {
            var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (locationStatus != PermissionStatus.Granted)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

            var cameraPermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraPermission != PermissionStatus.Granted)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var storagePermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storagePermission != PermissionStatus.Granted)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
        }
    }
}
