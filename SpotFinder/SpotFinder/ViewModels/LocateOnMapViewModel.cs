using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.Core;
using SpotFinder.DataServices;
using SpotFinder.Repositories;
using SpotFinder.Resx;
using System;
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

            IsBusy = false;
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

            reportManager.AddingPlace.Location.Latitude = centerPosition.Latitude;
            reportManager.AddingPlace.Location.Longitude = centerPosition.Longitude;

            IsBusy = true;

            int result = await PlaceRepository.SendAsync(reportManager.AddingPlace);

            if (result == 0)
            {
                var alertResult = await CurrentPage.DisplayAlert("Error.", "Problem with publishing spot. Try again or add to offline list", "Try again", "Add local");

                if(alertResult == true)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    LocalPlaceRepository.InsertPlace(reportManager.AddingPlace);
                }
                
            }

            var aaa = LocalPlaceRepository.GetAllPlaces();

            IsBusy = false;

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                await CurrentPage.Navigation.PopToRootAsync();
            
            else
            {
                //On phone (windows) Navigation.PopToRootAsync() does not work!
                var stackCount = CurrentPage.Navigation.NavigationStack.Count;
                for (int i = 0; i < stackCount - 1; i++)
                {
                    if (CurrentPage.Navigation.NavigationStack.Count == 0)
                        break;

                    await CurrentPage.Navigation.PopAsync();
                }
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
            loadingStackLayout.SetBinding(StackLayout.IsVisibleProperty, "IsBusy");

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

            Position position;

            if (Device.RuntimePlatform == Device.Windows)
                position = new Position(51, 19);
            else
                position = new Position(reportManager.AddingPlace.Location.Latitude, reportManager.AddingPlace.Location.Longitude);

            map = new Map(
            MapSpan.FromCenterAndRadius(
                    position, Distance.FromMiles(0.05)))
            {
                IsShowingUser = true,
                HeightRequest = 2048,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(6, 0, 6, 0)
            };

            if(Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                map.MapType = MapType.Satellite;
            }

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
