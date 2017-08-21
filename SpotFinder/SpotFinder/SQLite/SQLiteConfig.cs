using SpotFinder.SQLite.Models;
using SQLite;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder.SQLite
{
    public class SQLiteConfig
    {
        private readonly SQLiteConnection _sqLiteConnection;

        private static readonly SQLiteConfig sqliteConfig = new SQLiteConfig();

        public static SQLiteConfig Instance
        {
            get => sqliteConfig;
        }

        public static void Start()
        {
            var instantce = SQLiteConfig.Instance;
        }

        protected SQLiteConfig()
        {
            _sqLiteConnection = DependencyService.Get<ISQLite>().GetConnection();
            _sqLiteConnection.CreateTable<SQLitePlace>();
        }
        
    }
}
