
using SQLite;

namespace SpotFinder.SQLite.Models
{
    public class SQLitePlace
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PlaceJson { get; set; }
    }
}
