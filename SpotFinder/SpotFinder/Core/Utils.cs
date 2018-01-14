using SpotFinder.Models.Core;
using SpotFinder.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace SpotFinder.Core
{
    static class Utils
    {
        public static ImageSource Base64ImageToImageSource(string base64Image)
        {
            var imageBytes = Convert.FromBase64String(base64Image);
            return ImageSource.FromStream(() => { return new MemoryStream(imageBytes); });
        }

        public static string FirstLetterToUpperCase(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static PlaceWeb PlaceToPlaceWeb(Place place)
        {
            var webImagesList = new List<ImageWeb>();
            int imageId = 0;
            foreach (var base64Image in place.PhotosBase64List)
            {
                var imgDTO = new ImageWeb
                {
                    Id = imageId++,
                    Image = base64Image
                };
                webImagesList.Add(imgDTO);
            }

            return new PlaceWeb
            {
                Id = place.Id,
                Description = place.Description,
                Location = new Location(place.Location.Latitude, place.Location.Longitude),
                Type = place.Type,
                Name = place.Name,
                Images = webImagesList,
                Version = place.Version,

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
                Pyramid = place.Pyramid,
                Rail = place.Rail,
                Wallride = place.Wallride,
                OpenYourMind = place.OpenYourMind,
                Stairs = place.Stairs,
                UserId = place.UserId
            };
        }

        public static Place PlaceWebToPlace(PlaceWeb placeWeb)
        {
            var base64ImagesList = new List<string>();

            foreach(var tempImageWeb in placeWeb.Images)
            {
                base64ImagesList.Add(tempImageWeb.Image);
            }

            var place = new Place
            {
                Description = placeWeb.Description,
                Location = new Location(placeWeb.Location.Latitude, placeWeb.Location.Longitude),
                Type = placeWeb.Type,
                Name = placeWeb.Name,
                PhotosBase64List = base64ImagesList,
                Version = placeWeb.Version,

                Bank = placeWeb.Bank,
                Bowl = placeWeb.Bowl,
                Corners = placeWeb.Corners,
                Curb = placeWeb.Curb,
                Downhill = placeWeb.Downhill,
                Gap = placeWeb.Gap,
                Handrail = placeWeb.Handrail,
                Hubba = placeWeb.Hubba,
                Ledge = placeWeb.Ledge,
                Manualpad = placeWeb.Manualpad,
                Pyramid = placeWeb.Pyramid,
                Rail = placeWeb.Rail,
                Wallride = placeWeb.Wallride,
                OpenYourMind = placeWeb.OpenYourMind,
                Stairs = placeWeb.Stairs
            };

            if (placeWeb.Id == null)
                place.Id = 0;
            else
                place.Id = (int)placeWeb.Id;

            return place;
        }

        public static Place PlaceWebLightToPlace(PlaceWebLight placeWebLight)
        {
            var place = new Place();

            place.Description = placeWebLight.Description;
            place.Name = placeWebLight.Name;
            place.Id = placeWebLight.Id;
            place.Location = new Location(placeWebLight.Location.Latitude, placeWebLight.Location.Longitude);
            place.Type = placeWebLight.Type;

            place.PhotosBase64List = new List<string>();
            place.PhotosBase64List.Add(placeWebLight.MainPhoto);
            place.Version = placeWebLight.Version;

            return place;
        }
    }
}
