using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.Repositories;
using SpotFinder.Resx;
using SpotFinder.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.ViewModels
{
    public class MapPageViewModel : BaseViewModel
    {
        private IPlaceService PlaceRepository { get; }
        private ILocalPlaceRepository LocalPlaceRepository { get; }
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];
        private ContentPage CurrentPage { get; set; }
        private Map map;
        private IList<Place> allPlaces;

        private StackLayout mainStackLayout;
        private StackLayout loadingStackLayout;

        public MapPageViewModel(IPlaceService placeRepository, ILocalPlaceRepository localPlaceRepository)
        {
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in MapPageViewModel");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in MapPageViewModel");

            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();

            reportManager.StartEvent += StartLoading;
            reportManager.StopEvent += StopLoading;

            mainStackLayout = CreateMapLayout();
        }

        public void InjectPage(ContentPage contentPage, string pageTitle)
        {
            CurrentPage = contentPage;
            CurrentPage.Title = pageTitle;
            ShowMap();
        }

        public void ShowMap()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];
            if (mainStackLayout == null)
                CurrentPage.Content = CreateMapLayout();
            else
                CurrentPage.Content = mainStackLayout;
        }

        public void StartLoading()
        {
            IsBusy = true;
        }

        public void StopLoading()
        {
            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();
            allPlaces = reportManager.DownloadedPlaces;
            Device.BeginInvokeOnMainThread(() => { UpdateMapPins(); });
            IsBusy = false;
        }

        public void GetSpotsTest()
        {
            IsBusy = true;
        }

        public void UpdateMapPins()
        {
            if (allPlaces == null || allPlaces.Count == 0)
                return;

            map.Pins.Clear();

            foreach (var place in allPlaces)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(place.Location.Latitude, place.Location.Longitude),
                    Label = place.Name + " ",
                    Address = place.Description + " "
                };

                pin.Clicked += (s, e) =>
                {
                    CurrentPage.Navigation.PushAsync(new PlaceDetailsPage(place));
                };

                map.Pins.Add(pin);
            }
        }

        private StackLayout CreateMapLayout()
        {         
            if (map == null)
                map = new Map(
                MapSpan.FromCenterAndRadius(new Position(51.75924850, 19.45598330), Distance.FromMiles(0.3)))
                {
                    HeightRequest = 2480,
                    WidthRequest = 960,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

            loadingStackLayout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Refreshing spots on map...",
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

            return Utils.CreateItemOnItemLayout(map, loadingStackLayout);
        }
    }
}
