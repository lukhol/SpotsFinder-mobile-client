using SpotFinder.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceDetailsPage : ContentPage
    {
        private Place place;
        public PlaceDetailsPage(Place place)
        {
            this.place = place;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Content = new ScrollView
            {
                Content = CreateAllLayout()
            };
        }

        private StackLayout CreateAllLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = place.Name,
                        TextColor = Color.White, 
                        FontSize = 25,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    },
                    CreateTypeLayout(),
                    CreateGridSeparator(),
                    CreateObstacleLayout(),
                    CreateGridSeparator(),
                    CreateLocationLayout(),
                    CreateGridSeparator(),
                    CreateMainPhotoLayout()
                }
            };
            return layout;
        }

        private StackLayout CreateTypeLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Type:",
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(12, 0, 12, 0)
                    },
                    new Label
                    {
                        Text = "-" + place.Type.ToString(),
                        TextColor = Color.White,
                        Margin = new Thickness(15, 0 ,12 ,0)
                    }
                }
            };
            return layout;
        }

        private StackLayout CreateLocationLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Location:",
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(12, 0, 12, 0)
                    },
                    new Label
                    {
                        Text = "Latitude: " + place.Location.Latitude.ToString(),
                        TextColor = Color.White,
                        Margin = new Thickness(15, 0 ,12 ,0)
                    },
                    new Label
                    {
                        Text = "Longitude: " + place.Location.Longitude.ToString(),
                        TextColor = Color.White,
                        Margin = new Thickness(15, 0 ,12 ,0)
                    }
                }
            };
            return layout;
        }

        private Grid CreateGridSeparator()
        {
            var grid = new Grid
            {
                HeightRequest = 2,
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(12, 0, 12, 0)
            };
            return grid;
        }

        private StackLayout CreateObstacleLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Obstacles:",
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(12, 0, 12, 0)
                    }
                }
            };

            if (place.Stairs == true)
                layout.Children.Add(CreateObstacleLabel("-Stairs"));

            if (place.Rail == true)
                layout.Children.Add(CreateObstacleLabel("-Rail"));

            if (place.Ledge == true)
                layout.Children.Add(CreateObstacleLabel("-Ledge"));

            if (place.Handrail == true)
                layout.Children.Add(CreateObstacleLabel("-Handrail"));

            if (place.Corners == true)
                layout.Children.Add(CreateObstacleLabel("-Corners"));

            if (place.Manualpad == true)
                layout.Children.Add(CreateObstacleLabel("-Manualpad"));

            if (place.Wallride == true)
                layout.Children.Add(CreateObstacleLabel("-Wallride"));

            if (place.Downhill == true)
                layout.Children.Add(CreateObstacleLabel("-Downhill"));

            if (place.OpenYourMind == true)
                layout.Children.Add(CreateObstacleLabel("-OpenYourMind"));

            if (place.Pyramid == true)
                layout.Children.Add(CreateObstacleLabel("-Pyramid"));

            if (place.Curb == true)
                layout.Children.Add(CreateObstacleLabel("-Curb"));

            if (place.Bank == true)
                layout.Children.Add(CreateObstacleLabel("-Bank"));

            if (place.Bowl == true)
                layout.Children.Add(CreateObstacleLabel("-Bowl"));

            return layout;
        }

        private Label CreateObstacleLabel(string text)
        {
            var label = new Label
            {
                TextColor = Color.White,
                Text = text,
                Margin = new Thickness(15, 0, 12, 0)
            };
            return label;
        }

        private StackLayout CreateMainPhotoLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = place.MainPhoto
                    }
                }
            };

            foreach(var base64Image in place.Photos)
            {
                var image = new Image
                {
                    Source = Utils.Base64ImageToImageSource(base64Image)
                };
                layout.Children.Add(image);
            }

            return layout;
        }
    }
}