using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotFinder.SQLite.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder.Core
{
    public class LocalPlaceRepository : ILocalPlaceRepository
    {
        private readonly SQLiteConnection SQLiteConnection;

        public LocalPlaceRepository()
        {
            SQLiteConnection = DependencyService.Get<ISQLite>().GetConnection();
        }

        public Place GetPlace(int id)
        {
            var list = SQLiteConnection.Query<SQLitePlace>("SELECT * FROM SQLitePlace WHERE Id = ?", id);

            SQLitePlace sqlitePlace;

            if (list != null && list.Count > 0)
                sqlitePlace = list.FirstOrDefault();
            else
                return null;

            return FromSQLitePlaceToCorePlace(sqlitePlace);            
        }

        public async Task<bool> InsertPlace(string placeJson)
        {
            try
            {
                SQLiteConnection.Insert(new SQLitePlace { PlaceJson = placeJson });
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }

            return true;
        }

        public async Task<List<Place>> GetAllPlaces()
        {
            var allSpots = new List<Place>();

            var list = SQLiteConnection.Query<SQLitePlace>("SELECT * FROM SQLitePlace");

            SQLitePlace sqlitePlace;

            if (list != null && list.Count > 0)
                sqlitePlace = list.FirstOrDefault();
            else
                return null;

            foreach(var item in list)
            {
                allSpots.Add(FromSQLitePlaceToCorePlace(item));
            }

            return allSpots;
        }

        private Place FromSQLitePlaceToCorePlace(SQLitePlace sqlitePlace)
        {
            var placeJson = sqlitePlace.PlaceJson;

            if (string.IsNullOrEmpty(placeJson))
                return null;

            var results = JsonConvert.DeserializeObject<Place>(placeJson);

            return results;         
        }
    }
}
