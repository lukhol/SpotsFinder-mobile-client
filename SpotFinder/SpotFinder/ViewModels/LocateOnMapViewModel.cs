using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.Core;
using SpotFinder.DataServices;
using SpotFinder.Repositories;
using SpotFinder.Resx;
using SpotFinder.Views.Root;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.ViewModels
{
    public class LocateOnMapViewModel : BaseViewModel
    {
        private ContentPage CurrentPage { get; set; }
        private IPlaceService PlaceRepository { get; }
        private ILocalPlaceRepository LocalPlaceRepository { get; }
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];
        private Map map;
        private StackLayout loadingStackLayout;

        public LocateOnMapViewModel(IPlaceService placeRepository, ILocalPlaceRepository localPlaceRepository)
        {
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in LocateOnMapViewModel");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in LocateOnMapViewModel");

            IsBussy = false;
        }

        public void InjectPage(ContentPage page)
        {
            CurrentPage = page;
            CurrentPage.Content = CreateMainLayout();
        }

        public async void ReportAsync()
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

            IsBussy = true;

            int result = await PlaceRepository.SendAsync(reportManager.Place);

            if (result == 0)
            {
                var alertResult = await CurrentPage.DisplayAlert("Error.", "Problem with publishing spot. Try again or add to offline list", "Try again", "Add local");

                if(alertResult == true)
                {
                    IsBussy = false;
                    return;
                }
                else
                {
                    await LocalPlaceRepository.InsertPlaceAsync(reportManager.Place);
                }
                
            }

            var aaa = await LocalPlaceRepository.GetAllPlacesAsync();

            IsBussy = false;

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                await CurrentPage.Navigation.PopToRootAsync();
            else
            {
                //On phone (windows) Navigation.PopToRootAsync() does not work!
                var stackCount = CurrentPage.Navigation.NavigationStack.Count;
                for (int i = 0; i < stackCount; i++)
                    await CurrentPage.Navigation.PopAsync();
            }
        }

        public ICommand LocateCommand => new Command(ReportAsync);

        private StackLayout CreateMainLayout()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];

            loadingStackLayout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Uploading spot...",
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

            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();

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
            var button = Utils.CreateDownSiteButton(LocateCommand, AppResources.LocateCommandTitle, new Thickness(5, 5, 5, 12));

            button.MinimumHeightRequest = 80;

            layout.Children.Add(label);
            layout.Children.Add(Utils.CreateItemOnItemLayout(map, aim, aimTwo));
            layout.Children.Add(button);

            return Utils.CreateItemOnItemLayout(layout, loadingStackLayout);
        }
    }
}
