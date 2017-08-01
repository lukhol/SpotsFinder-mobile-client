using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static Grid CreateGridSeparator(int marginSiteValue)
        {
            return new Grid
            {
                HeightRequest = 2,
                BackgroundColor = Color.White,
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

        public static StackLayout CreateGridButton(Command ClickCommand, string text, int marginValue = 0)
        {
            Thickness spaceThickness;
            if (marginValue == 0)
                spaceThickness = new Thickness(12, 12, 12, 12);
            else
                spaceThickness = new Thickness(marginValue);

            var grid = new Grid
            {
                BackgroundColor = Color.White, 
                Margin = spaceThickness,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var boxView = new BoxView
            {
                BackgroundColor = Color.White
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
                TextColor = Color.White,
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

        public static StackLayout CreateItemOnItemLayout(params View[] views)
        {
            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
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
    }
}
