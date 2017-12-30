using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly ISQLite SQLite;

        public PlaceRepository(ISQLite sqlite)
        {
            SQLite = sqlite ?? throw new ArgumentNullException(nameof(ISQLite));
        }

        public Place GetPlace(int id)
        {
            using (var connection = SQLite.GetConnection())
            {
                var list = connection.Query<SQLitePlace>("SELECT * FROM SQLitePlace WHERE Id = ?", id);

                if (list == null || list.Count == 0)
                    throw new NotFoundPlaceException(string.Format("Not found place with id: {0} in local database.", id));
                
                return FromSQLitePlaceToPlace(list.FirstOrDefault());
            }
        }

        public bool InsertPlace(Place place)
        {
            if (place.Id == 0)
                return false;

            try
            {
                using (var connection = SQLite.GetConnection())
                {
                    if (!ExistPlace(place.Id))
                    {
                        var sqlitePlace = FromPlaceToSQLitePlace(place);
                        connection.Insert(sqlitePlace);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }

            return true;
        }

        public bool ExistPlace(int id)
        {
            var query = "SELECT count(*) FROM SQLitePlace WHERE Id = ?";
            using (var connection = SQLite.GetConnection())
            {
                var command = connection.CreateCommand(query, id);
                int count = command.ExecuteScalar<int>();
                return count == 0 ? false : true;
            }
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
                Location = new Location(sqlitePlace.Latitude, sqlitePlace.Longitude),
                PhotosBase64List = listOfImages,
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
                Id = place.Id,
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

            if (place.PhotosBase64List != null && place.PhotosBase64List.Count > 0)
                sqlitePlace.Image1 = place.PhotosBase64List.ElementAt(0);

            if (place.PhotosBase64List != null && place.PhotosBase64List.Count > 1)
                sqlitePlace.Image2 = place.PhotosBase64List.ElementAt(1);

            if (place.PhotosBase64List != null && place.PhotosBase64List.Count > 2)
                sqlitePlace.Image3 = place.PhotosBase64List.ElementAt(2);

            if (place.PhotosBase64List != null && place.PhotosBase64List.Count > 3)
                sqlitePlace.Image4 = place.PhotosBase64List.ElementAt(3);

            if (place.PhotosBase64List != null && place.PhotosBase64List.Count > 4)
                sqlitePlace.Image5 = place.PhotosBase64List.ElementAt(4);

            if (place.PhotosBase64List != null && place.PhotosBase64List.Count > 5)
                sqlitePlace.Image5 = place.PhotosBase64List.ElementAt(5);

            return sqlitePlace;
        }
    }
}
