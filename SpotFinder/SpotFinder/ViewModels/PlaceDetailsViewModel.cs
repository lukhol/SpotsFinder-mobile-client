using SpotFinder.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.ViewModels
{
    public class PlaceDetailsViewModel
    {
        private INavigation Navigation { get; }
        private Place place;

        private Command buttonCommand;
        private string textOnButton;

        public PlaceDetailsViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public void Initialize(Place place, Command buttonCommand = null, string textOnButton = null)
        {
            this.place = place;

            if(buttonCommand != null)
            {
                this.textOnButton = textOnButton;
                this.buttonCommand = buttonCommand;
            }

            var currPage = (ContentPage)Navigation.NavigationStack[Navigation.NavigationStack.Count - 1];
            currPage.Content = new ScrollView
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
                    Utils.CreateGridSeparator(12),
                    CreateDescriptionLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateObstacleLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateLocationLayout(),
                    Utils.CreateGridSeparator(12),
                    CreatePhotoLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateMapLayout(),
                    Utils.CreateGridSeparator(12),
                    Utils.CreateGridButton(new Command(() => 
                    {
                        Navigation.PopAsync();
                    }), "Go back")
                }
            };

            if(buttonCommand != null)
            {
                layout.Children.Add(Utils.CreateGridSeparator(12));
                var button = Utils.CreateGridButton(buttonCommand, textOnButton);
                layout.Children.Add(button);
            }

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

        private StackLayout CreatePhotoLayout()
        {
            var layout = new StackLayout();
            foreach (var base64Image in place.PhotosBase64)
            {
                var image = new Image
                {
                    Source = Utils.Base64ImageToImageSource(base64Image)
                };
                image.Margin = new Thickness(5, 5, 5, 5);
                layout.Children.Add(image);
            }

            return layout;
        }

        private StackLayout CreateDescriptionLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Description:",
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(12, 0, 12, 0)
                    },
                    new Label
                    {
                        Text = place.Description,
                        TextColor = Color.White,
                        Margin = new Thickness(15, 0 ,12 ,0)
                    }
                }
            };

            return layout;
        }

        private StackLayout CreateMapLayout()
        {
            var map = new Map(
            MapSpan.FromCenterAndRadius(new Position(place.Location.Latitude, place.Location.Longitude), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(place.Location.Latitude, place.Location.Longitude),
                Label = place.Name + " ",
                Address = place.Description + " "
            };

            map.Pins.Add(pin);

            var layout = new StackLayout
            {
                Children = { map },
                HeightRequest = 500,
                Margin = new Thickness(12)
            };

            return layout;
        }
    }
}
