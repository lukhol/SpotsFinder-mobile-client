using SpotFinder.Models.Core;
using SpotFinder.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.Core
{
    static class Utils
    {
        static Xamarin.Forms.Color mainAccentColor = Xamarin.Forms.Color.White;

        public static ImageSource Base64ImageToImageSource(string base64Image)
        {
            var imageBytes = Convert.FromBase64String(base64Image);
            return ImageSource.FromStream(() => { return new MemoryStream(imageBytes); });
        }

        public static Grid CreateGridSeparator(int marginSiteValue)
        {
            return new Grid
            {
                HeightRequest = 2,
                BackgroundColor = mainAccentColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(marginSiteValue, 0, marginSiteValue, 0)
            };
        }

        public static StackLayout CreateHorizontalStackLayout(params View[] obj)
        {
            var layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            foreach (var item in obj)
            {
                layout.Children.Add(item);
            }

            return layout;
        }

        public static StackLayout CreateGridButton(ICommand ClickCommand, string text, int marginValue = 0)
        {
            Thickness spaceThickness;
            if (marginValue == 0)
                spaceThickness = new Thickness(12, 12, 12, 12);
            else
                spaceThickness = new Thickness(marginValue);

            var grid = new Grid
            {
                BackgroundColor = mainAccentColor, 
                Margin = spaceThickness,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var boxView = new BoxView
            {
                BackgroundColor = mainAccentColor
            };
            var innerGrid = new Grid
            {
                BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"],
                Margin = new Thickness(1)
            };
            var messageOnButton = new Label
            {
                Text = text,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = mainAccentColor,
                Margin = new Thickness(10, 10, 10, 10)
            };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Command = ClickCommand;
            innerGrid.GestureRecognizers.Add(tapGestureRecognizer);

            innerGrid.Children.Add(messageOnButton);
            grid.Children.Add(boxView);
            grid.Children.Add(innerGrid);

            return new StackLayout
            {
                Children =
                {
                    grid
                }
            };
        }
        
        public static Button CreateDownSiteButton(ICommand ClickCommand, string text, Thickness spaceThickness)
        {
            var button = new Button
            {
                Command = ClickCommand,
                Text = text,
                Margin = spaceThickness,
                BorderRadius = 20,
                BorderWidth = 1,
                BackgroundColor = Color.FromHex("0962c1"),
                TextColor = mainAccentColor,
                BorderColor = mainAccentColor
            };

            return button;
        }

        public static StackLayout CreateItemOnItemLayout(params View[] views)
        {
            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            foreach(var view in views)
                grid.Children.Add(view, 0, 0);

            return new StackLayout
            {
                Children = { grid }
            };
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
            foreach (var base64Image in place.PhotosBase64)
            {
                var img = new ImageWeb
                {
                    Id = imageId++,
                    Image = base64Image
                };
                webImagesList.Add(img);
            }

            return new PlaceWeb
            {
                Id = place.Id,
                Description = place.Description,
                Location = new Location
                {
                    Latitude = place.Location.Latitude,
                    Longitude = place.Location.Longitude
                },
                Type = place.Type,
                Name = place.Name,
                Images = webImagesList,
                
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
                Stairs = place.Stairs
            };
        }

        public static Place PlaceWebToPlace(PlaceWeb placeWeb)
        {
            var base64ImagesList = new List<string>();

            foreach(var tempImageWeb in placeWeb.Images)
            {
                base64ImagesList.Add(tempImageWeb.Image);
            }

            var place =  new Place
            {
                Description = placeWeb.Description,
                Location = new Location
                {
                    Latitude = placeWeb.Location.Latitude,
                    Longitude = placeWeb.Location.Longitude
                },
                Type = placeWeb.Type,
                Name = placeWeb.Name,
                PhotosBase64 = base64ImagesList,

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
            place.Location.Latitude = placeWebLight.Location.Latitude;
            place.Location.Longitude = placeWebLight.Location.Longitude;
            place.Type = placeWebLight.Type;

            place.PhotosBase64 = new List<string>();
            place.PhotosBase64.Add(placeWebLight.MainPhoto);

            return place;
        }
    }
}
