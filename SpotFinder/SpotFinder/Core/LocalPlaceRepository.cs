using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotFinder.Core.Enums;
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
            var list = SQLiteConnection.Query<SQLitePlaceLocal>("SELECT * FROM SQLitePlaceLocal WHERE Id = ?", id);

            if (list == null || list.Count == 0)
                return null;

            return FromSQLitePlaceLocalToPlace(list.FirstOrDefault());            
        }

        public async Task<bool> InsertPlaceAsync(Place place)
        {
            try
            {
                SQLiteConnection.Insert(FromPlaceToSQLitePlaceLocal(place));
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }

            return true;
        }

        public async Task<List<Place>> GetAllPlacesAsync()
        {
            var allSpots = new List<Place>();

            var list = SQLiteConnection.Query<SQLitePlaceLocal>("SELECT * FROM SQLitePlaceLocal");

            if (list == null || list.Count == 0)
                return null;

            foreach(var item in list)
            {
                allSpots.Add(FromSQLitePlaceLocalToPlace(item));
            }

            return allSpots;
        }

        private Place FromSQLitePlaceLocalToPlace(SQLitePlaceLocal sqlitePlaceLocal)
        {
            var listOfImages = new List<string>();

            if (!string.IsNullOrEmpty(sqlitePlaceLocal.Image1))
                listOfImages.Add(sqlitePlaceLocal.Image1);

            if (!string.IsNullOrEmpty(sqlitePlaceLocal.Image2))
                listOfImages.Add(sqlitePlaceLocal.Image2);

            if (!string.IsNullOrEmpty(sqlitePlaceLocal.Image3))
                listOfImages.Add(sqlitePlaceLocal.Image3);

            if (!string.IsNullOrEmpty(sqlitePlaceLocal.Image4))
                listOfImages.Add(sqlitePlaceLocal.Image4);

            if (!string.IsNullOrEmpty(sqlitePlaceLocal.Image5))
                listOfImages.Add(sqlitePlaceLocal.Image5);

            PlaceType placeType;

            switch (sqlitePlaceLocal.Type)
            {
                case 0:
                    placeType = PlaceType.Skatepark;
                    break;

                case 1:
                    placeType = PlaceType.Skatespot;
                    break;

                case 2:
                    placeType = PlaceType.DIY;
                    break;

                default:
                    placeType = PlaceType.Skatespot;
                    break;
            }

            return new Place
            {
                Id = sqlitePlaceLocal.Id,
                Name = sqlitePlaceLocal.Name,
                Type = placeType,
                Description = sqlitePlaceLocal.Description,
                Location = new Location
                {
                    Latitude = sqlitePlaceLocal.Latitude,
                    Longitude = sqlitePlaceLocal.Longitude
                },
                PhotosBase64 = listOfImages,

                Bank = sqlitePlaceLocal.Bank,
                Bowl = sqlitePlaceLocal.Bowl,
                Corners = sqlitePlaceLocal.Corners,
                Curb = sqlitePlaceLocal.Curb,
                Downhill = sqlitePlaceLocal.Downhill,
                Gap = sqlitePlaceLocal.Gap,
                Handrail = sqlitePlaceLocal.Handrail,
                Hubba = sqlitePlaceLocal.Hubba,
                Ledge = sqlitePlaceLocal.Ledge,
                Manualpad = sqlitePlaceLocal.Manualpad,
                OpenYourMind = sqlitePlaceLocal.OpenYourMind,
                Pyramid = sqlitePlaceLocal.Pyramid,
                Rail = sqlitePlaceLocal.Rail,
                Stairs = sqlitePlaceLocal.Stairs,
                Wallride = sqlitePlaceLocal.Wallride
            };
        }

        private SQLitePlaceLocal FromPlaceToSQLitePlaceLocal(Place place)
        {
            var sqlitePlaceLocal = new SQLitePlaceLocal
            {
                Description = place.Description,
                Name = place.Name,
                Latitude = place.Location.Latitude,
                Longitude = place.Location.Longitude,

                Bank = place.Bank,
                Bowl = place.Bowl,
                Corners = place.Corners,
                Curb = place.Curb,
                Downhill = place.Downhill,
                Gap = place.Gap,
                Handrail = place.Handrail,
                Hubba = place.Hubba,
                Ledge = place.Ledge,
                Manualpad = place.Manualpad,
                OpenYourMind = place.OpenYourMind,
                Pyramid = place.Pyramid,
                Rail = place.Rail,
                Stairs = place.Stairs,
                Wallride = place.Wallride
            };

            switch (place.Type)
            {
                case PlaceType.Skatepark:
                    sqlitePlaceLocal.Type = 0;
                    break;

                case PlaceType.Skatespot:
                    sqlitePlaceLocal.Type = 1;
                    break;

                default:
                    sqlitePlaceLocal.Type = 2;
                    break;
            }

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 0)
                sqlitePlaceLocal.Image1 = place.PhotosBase64.ElementAt(0);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 1)
                sqlitePlaceLocal.Image2 = place.PhotosBase64.ElementAt(1);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 2)
                sqlitePlaceLocal.Image3 = place.PhotosBase64.ElementAt(2);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 3)
                sqlitePlaceLocal.Image4 = place.PhotosBase64.ElementAt(3);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 4)
                sqlitePlaceLocal.Image5 = place.PhotosBase64.ElementAt(4);

            return sqlitePlaceLocal;
        }

        private Place FromSQLitePlaceToPlace(SQLitePlace sqlitePlace)
        {
            var listOfImages = new List<string>();

            if (!string.IsNullOrEmpty(sqlitePlace.Image1))
                listOfImages.Add(sqlitePlace.Image1);

            if (!string.IsNullOrEmpty(sqlitePlace.Image2))
                listOfImages.Add(sqlitePlace.Image2);

            if (!string.IsNullOrEmpty(sqlitePlace.Image3))
                listOfImages.Add(sqlitePlace.Image3);

            if (!string.IsNullOrEmpty(sqlitePlace.Image4))
                listOfImages.Add(sqlitePlace.Image4);

            if (!string.IsNullOrEmpty(sqlitePlace.Image5))
                listOfImages.Add(sqlitePlace.Image5);

            if (!string.IsNullOrEmpty(sqlitePlace.Image6))
                listOfImages.Add(sqlitePlace.Image6);

            PlaceType placeType;

            switch (sqlitePlace.Type)
            {
                case 0:
                    placeType = PlaceType.Skatepark;
                    break;

                case 1:
                    placeType = PlaceType.Skatespot;
                    break;

                case 2:
                    placeType = PlaceType.DIY;
                    break;

                default:
                    placeType = PlaceType.Skatespot;
                    break;
            }

            return new Place
            {
                Id = sqlitePlace.Id,
                Name = sqlitePlace.Name,
                Type = placeType,
                Description = sqlitePlace.Description,
                Location = new Location
                {
                    Latitude = sqlitePlace.Latitude,
                    Longitude = sqlitePlace.Longitude
                },
                PhotosBase64 = listOfImages,

                Bank = sqlitePlace.Bank,
                Bowl = sqlitePlace.Bowl,
                Corners = sqlitePlace.Corners,
                Curb = sqlitePlace.Curb,
                Downhill = sqlitePlace.Downhill,
                Gap = sqlitePlace.Gap,
                Handrail = sqlitePlace.Handrail,
                Hubba = sqlitePlace.Hubba,
                Ledge = sqlitePlace.Ledge,
                Manualpad = sqlitePlace.Manualpad,
                OpenYourMind = sqlitePlace.OpenYourMind,
                Pyramid = sqlitePlace.Pyramid,
                Rail = sqlitePlace.Rail,
                Stairs = sqlitePlace.Stairs,
                Wallride = sqlitePlace.Wallride
            };
        }

        private SQLitePlace FromPlaceToSQLitePlace(Place place)
        {
            var sqlitePlace = new SQLitePlace
            {
                Description = place.Description,
                Name = place.Name,
                Latitude = place.Location.Latitude,
                Longitude = place.Location.Longitude,

                Bank = place.Bank,
                Bowl = place.Bowl,
                Corners = place.Corners,
                Curb = place.Curb,
                Downhill = place.Downhill,
                Gap = place.Gap,
                Handrail = place.Handrail,
                Hubba = place.Hubba,
                Ledge = place.Ledge,
                Manualpad = place.Manualpad,
                OpenYourMind = place.OpenYourMind,
                Pyramid = place.Pyramid,
                Rail = place.Rail,
                Stairs = place.Stairs,
                Wallride = place.Wallride
            };

            switch (place.Type)
            {
                case PlaceType.Skatepark:
                    sqlitePlace.Type = 0;
                    break;

                case PlaceType.Skatespot:
                    sqlitePlace.Type = 1;
                    break;

                default:
                    sqlitePlace.Type = 2;
                    break;
            }

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 0)
                sqlitePlace.Image1 = place.PhotosBase64.ElementAt(0);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 1)
                sqlitePlace.Image2 = place.PhotosBase64.ElementAt(1);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 2)
                sqlitePlace.Image3 = place.PhotosBase64.ElementAt(2);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 3)
                sqlitePlace.Image4 = place.PhotosBase64.ElementAt(3);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 4)
                sqlitePlace.Image5 = place.PhotosBase64.ElementAt(4);

            if (place.PhotosBase64 != null && place.PhotosBase64.Count > 5)
                sqlitePlace.Image5 = place.PhotosBase64.ElementAt(5);

            return sqlitePlace;
        }

        public Place GetPlaceOryginal(int id)
        {
            var list = SQLiteConnection.Query<SQLitePlace>("SELECT * FROM SQLitePlace WHERE Id = ?", id);

            if (list == null || list.Count == 0)
            {
                return null;
            }

            return FromSQLitePlaceToPlace(list.FirstOrDefault());
        }
    }
}
