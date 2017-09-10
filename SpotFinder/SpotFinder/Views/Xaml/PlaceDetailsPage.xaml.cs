using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.Models.Core;
using SpotFinder.ViewModels.Xaml;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Xaml
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceDetailsPage : ContentPage
    {
        public PlaceDetailsPage()
        {
            InitializeComponent();
            BindingContext = new PlaceDetailsViewModel();

            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();

            reportManager.RequestDownloadPlace(26);
            reportManager.DownloadFinished += Update;

            PreapreListItemSelected();
        }

        public void Update()
        {
            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();
            var place = reportManager.CurrentPlaceToShow;
            if (place != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(place.Location.Latitude, place.Location.Longitude), Distance.FromMiles(0.3)));

                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(place.Location.Latitude, place.Location.Longitude),
                        Label = place.Name + " ",
                        Address = place.Description + " "
                    };

                    MyMap.Pins.Add(pin);

                    //var imageListViewSource = (List<ImageSource>)ImagesListView.ItemsSource;
                    //ImagesListView.HeightRequest = 300 * imageListViewSource.Count;

                    foreach (var item in place.PhotosBase64)
                    {
                        var image = new Image();
                        image.Source = Utils.Base64ImageToImageSource(item);
                            ImagesStackLayout.Children.Add(image);
                    }
                });
            }
        }

        private void PreapreListItemSelected()
        {
            /*
            ImagesListView.ItemSelected += (s, e) =>
            {
                ImagesListView.SelectedItem = null;
            };
            */

            ObstacleListView.ItemSelected += (s, e) =>
            {
                ObstacleListView.SelectedItem = null;
            };
        }
    }
}