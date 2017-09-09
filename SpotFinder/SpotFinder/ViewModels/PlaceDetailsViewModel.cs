using SpotFinder.Core;
using SpotFinder.Resx;
using SpotFinder.SQLite.Models;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder.ViewModels
{
    public class PlaceDetailsViewModel : BaseViewModel
    {
        private ContentPage CurrentPage;
        private Place place;
        private StackLayout mapLayout;
        private ScrollView scrollView;

        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];

        private Command buttonCommand;
        private string textOnButton;

        public async void Initialize(Place place, Command buttonCommand = null, string textOnButton = null)
        {
            if (buttonCommand != null)
            {
                this.place = place;
                scrollView.Content = CreateAllLayout();
                mapLayout.IsVisible = true;
                IsBussy = false;
            }
            else
            {
                this.textOnButton = textOnButton;
                this.buttonCommand = buttonCommand;

                int id = place.Id;
                var localPlaceRepository = new LocalPlaceRepository();
                var placeRepository = new PlaceRepository();

                this.place = localPlaceRepository.GetPlaceOryginal(id);

                if (this.place == null)
                    this.place = await placeRepository.GetPlaceById(id);

                scrollView.Content = CreateAllLayout();
                mapLayout.IsVisible = true;
                IsBussy = false;
            }

        }

        public void InjectPage(ContentPage contentPage, string pageTitle)
        {
            CurrentPage = contentPage;
            CurrentPage.Title = pageTitle;
            scrollView = new ScrollView();
            scrollView.Content = CreateLoadingLayout();
            IsBussy = true;
            CurrentPage.Content = new ScrollView
            {
                Content = scrollView
            };
        }

        private StackLayout CreateLoadingLayout()
        {
            var loadingStackLayout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Downloading spot...",
                        TextColor = mainAccentColor,
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new ActivityIndicator
                    {
                        IsVisible = true,
                        IsRunning = true,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.Center,
                        Color = mainAccentColor
                    }
                },
                IsVisible = false,
                BackgroundColor = Color.FromRgba(12, 12, 12, 200)
            };
            loadingStackLayout.SetBinding(StackLayout.IsVisibleProperty, "IsBussy");

            return loadingStackLayout;
        }

        private StackLayout CreateAllLayout()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];

            mapLayout = CreateMapLayout();
            mapLayout.IsVisible = false;

            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = place.Name,
                        TextColor = mainAccentColor,
                        FontSize = 25,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(12)
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
                    mapLayout,
                    Utils.CreateGridSeparator(12),
                    Utils.CreateDownSiteButton(new Command(() => 
                    {
                        CurrentPage.Navigation.PopAsync();
                    }), AppResources.GoBackCommandTitle, new Thickness(12))
                }
            };

            if(buttonCommand != null)
            {
                var button = Utils.CreateDownSiteButton(buttonCommand, textOnButton, new Thickness(12, 0, 12, 12));
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
                        Text = AppResources.TypeLabel,
                        TextColor = mainAccentColor,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(12, 0, 12, 0)
                    },
                    new Label
                    {
                        Text = "-" + place.Type.ToString(),
                        TextColor = mainAccentColor,
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
                        TextColor = mainAccentColor,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(12, 0, 12, 0)
                    },
                    new Label
                    {
                        Text = "Latitude: " + place.Location.Latitude.ToString(),
                        TextColor = mainAccentColor,
                        Margin = new Thickness(15, 0 ,12 ,0)
                    },
                    new Label
                    {
                        Text = "Longitude: " + place.Location.Longitude.ToString(),
                        TextColor = mainAccentColor,
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
                        Text = AppResources.ObstaclesLabel,
                        TextColor = mainAccentColor,
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

            if (place.Hubba == true)
                layout.Children.Add(CreateObstacleLabel("-Hubba"));

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
                TextColor = mainAccentColor,
                Text = text,
                Margin = new Thickness(15, 0, 12, 0)
            };
            return label;
        }

        private StackLayout CreatePhotoLayout()
        {
            var layout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var pageWidth = Application.Current.MainPage.Width;

            var firstImage = true;
            foreach (var base64Image in place.PhotosBase64)
            {
                if (firstImage)
                {
                    firstImage = false;
                    continue;
                }
                var image = new Image
                {
                    Source = Utils.Base64ImageToImageSource(base64Image),
                };
                image.Margin = new Thickness(5, 5, 5, 5);
                image.Aspect = Aspect.AspectFill;
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
                        Text = AppResources.DescriptionPlaceholder + ":",
                        TextColor = mainAccentColor,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(12, 0, 12, 0)
                    },
                    new Label
                    {
                        Text = place.Description,
                        TextColor = mainAccentColor,
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
                //IsShowingUser = true,
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
