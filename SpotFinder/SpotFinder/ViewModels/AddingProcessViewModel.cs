using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class AddingProcessViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation Navigation { get; }
        private double latitude;
        private double longitude;
        private bool canGoBack;

        public bool CanGoBack
        {
            get => canGoBack;
            set
            {
                canGoBack = value;
                OnPropertyChanged();
            }
        }

        public AddingProcessViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public async Task StartAsync()
        {
            CanGoBack = false;
            var currPage = (ContentPage)Navigation.NavigationStack[Navigation.NavigationStack.Count - 1];
            currPage.Content = CreateWaitingLayout();

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                if (position == null)
                {
                    return;
                }

                latitude = position.Latitude;
                longitude = position.Longitude;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("", e);
            }

            currPage.Content = CreateLocationLabels();
            CanGoBack = true;
        }

        private StackLayout CreateWaitingLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Acquiring location",
                        TextColor = Color.White,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand
                    },
                    new ActivityIndicator
                    {
                        IsRunning = true,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.StartAndExpand
                    }
                }
            };
            return layout;
        }

        private StackLayout CreateLocationLabels()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Latitude: " + latitude.ToString(),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        TextColor = Color.White
                    },
                    new Label
                    {
                        Text = "Longitude: " + longitude.ToString(),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        TextColor = Color.White
                    }
                }
            };
            return layout;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
