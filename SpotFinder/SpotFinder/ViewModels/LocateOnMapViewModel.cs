using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.Core;
using SpotFinder.Resx;
using SpotFinder.Views.Root;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.ViewModels
{
    public class LocateOnMapViewModel
    {
        private INavigation Navigation { get; }
        private ContentPage CurrentPage { get; set; }
        private IPlaceRepository PlaceRepository { get; }
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];
        private Map map;

        public LocateOnMapViewModel(INavigation navigation, IPlaceRepository placeRepository)
        {
            Navigation = navigation ?? throw new ArgumentNullException("navigation is null in LocateOnMapViewModel");
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in LocateOnMapViewModel");
        }

        public void InjectPage(ContentPage page)
        {
            CurrentPage = page;
            CurrentPage.Content = CreateMainLayout();
        }

        private Command LocateCommand => new Command(() =>
        {
            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var reportManager = (ReportManager)serviceLocator.GetService(typeof(ReportManager));

            Position centerPosition = new Position();

            if (map != null && map.VisibleRegion != null)
                centerPosition = map.VisibleRegion.Center;
            else
                return;

            reportManager.Place.Location.Latitude = centerPosition.Latitude;
            reportManager.Place.Location.Longitude = centerPosition.Longitude;

            PlaceRepository.Send(reportManager.Place);
            if(Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                Navigation.PopToRootAsync();
            else
            {
                //On phone (windows) Navigation.PopToRootAsync() does not work!
                var stackCount = Navigation.NavigationStack.Count;
                for (int i = 0; i < stackCount; i++)
                    Navigation.PopAsync();
            }
        });

        private StackLayout CreateMainLayout()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];

            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var reportManager = (ReportManager)serviceLocator.GetService(typeof(ReportManager));

            var layout = new StackLayout();

            var label = new Label
            {
                TextColor = mainAccentColor,
                Text = AppResources.UseMapLabel,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(5)
            };

            map = new Map(
            MapSpan.FromCenterAndRadius(
                    new Position(reportManager.Place.Location.Latitude, reportManager.Place.Location.Longitude), Distance.FromMiles(0.05)))
            {
                IsShowingUser = true,
                MapType = MapType.Satellite,
                HeightRequest = 2048,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(6, 0, 6, 0)
            };

            var aim = new BoxView
            {
                WidthRequest = 5,
                HeightRequest = 5,
                Color = Color.Black,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var aimTwo = new BoxView
            {
                WidthRequest = 25,
                HeightRequest = 25,
                Color = new Color(255, 255, 255, 0.5),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var button = Utils.CreateDownSiteButton(LocateCommand, AppResources.LocateCommandTitle, new Thickness(5,5,5,12));
            button.MinimumHeightRequest = 80;

            layout.Children.Add(label);
            layout.Children.Add(Utils.CreateItemOnItemLayout(map, aim, aimTwo));
            layout.Children.Add(button);

            return layout;
        }
    }
}
