using SpotFinder.SQLite.Models;
using SQLite;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder
{
    public partial class App : Application
    {
        private readonly SQLiteConnection _sqLiteConnection;

        public App()
        {
            _sqLiteConnection = DependencyService.Get<ISQLite>().GetConnection();
            _sqLiteConnection.CreateTable<SQLitePlace>();

            InitializeComponent();
            MainPage = new NavigationPage(new Views.TabbedPage1());
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
