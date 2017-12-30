using SQLite;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder.SQLite
{
    public class SQLiteConfig
    {
        private readonly SQLiteConnection _sqLiteConnection;

        public SQLiteConfig()
        {
            _sqLiteConnection = DependencyService.Get<ISQLite>().GetConnection();
            _sqLiteConnection.CreateTable<SQLitePlace>();
            _sqLiteConnection.CreateTable<SQLitePlaceLocal>();
        }
        
    }
}
