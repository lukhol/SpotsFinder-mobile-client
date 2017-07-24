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
    public class MapPageViewModel
    {
        private INavigation Navigation { get; }
        private IPlaceRepository PlaceRepository { get; }
        private ILocalPlaceRepository LocalPlaceRepository { get; }
        private ContentPage CurrentPage { get; set; }
        private Map map;

        public MapPageViewModel(INavigation navigation, IPlaceRepository placeRepository, ILocalPlaceRepository localPlaceRepository)
        {
            Navigation = navigation ?? throw new ArgumentNullException("navigation is null in MapPageViewModel");
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in MapPageViewModel");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in MapPageViewModel");
        }

        public void InjectPage(ContentPage contentPage, string pageTitle)
        {
            CurrentPage = contentPage;
            CurrentPage.Title = pageTitle;
            ShowMap();
        }

        public void ShowMap()
        {
            if(map == null)
            {
                CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];
                CurrentPage.Content = CreateMapLayout();

                var allPlaces = LocalPlaceRepository.GetAllPlaces();

                foreach (var place in allPlaces)
                {
                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(place.Location.Latitude, place.Location.Longitude),
                        Label = place.Name + " ",
                        Address = place.Description + " "
                    };
                    map.Pins.Add(pin);
                }
            }
        }

        private StackLayout CreateMapLayout()
        {
            StackLayout layout;

            map = new Map(
            MapSpan.FromCenterAndRadius(new Position(51.75924850, 19.45598330), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            layout = new StackLayout
            {
                Children =
                {
                    map,
                    new Button
                    {
                        VerticalOptions = LayoutOptions.End,
                        Text = "Center position",
                        Command = new Command (() => 
                        {
                            var visibleRegion = map.VisibleRegion;
                            var centerPosition = visibleRegion.Center;

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                var currPage = (TabbedPage)Navigation.NavigationStack[Navigation.NavigationStack.Count - 1];
                                var mapPage = (ContentPage)currPage.Children[3];
                                await mapPage.DisplayAlert("Alert!", centerPosition.Latitude.ToString()
                                    + ", " + centerPosition.Longitude.ToString(), "Ok");
                            });
                        })
                    }
                }
            };

            return layout;
        }

        public void TestDostaniaSrodkaMapy()
        {
            var visibleRegion = map.VisibleRegion;
            var centerPosition = visibleRegion.Center;
        }
    }
}
