using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.Models.Core;
using SpotFinder.Repositories;
using SpotFinder.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class LocalListViewModel : BaseViewModel
    {
        private ILocalPlaceRepository LocalPlaceRepository { get; }

        private ContentPage CurrentPage { get; set; }
        private StackLayout loadingStackLayout;
        private ListView listView;
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];

        private ObservableCollection<Place> observablePlaceList = new ObservableCollection<Place>();
        public ObservableCollection<Place> ObservablePlaceList
        {
            get => observablePlaceList;
            set
            {
                observablePlaceList = value;
                observablePlaceList.Add(new Place());
                observablePlaceList.Remove(observablePlaceList.Last());
                OnPropertyChanged();
            }
        }

        public LocalListViewModel(ILocalPlaceRepository localPlaceRepository)
        {
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in ListViewModel");
        }

        public void InjectPage(ContentPage contentPage)
        {
            CurrentPage = contentPage;
            CurrentPage.Content = CreateMainLayout();
            StopLoading();
        }

        public async void StopLoading()
        {
            var listOfLocalPlaces = await LocalPlaceRepository.GetAllPlacesAsync();
            if (listOfLocalPlaces == null || listOfLocalPlaces.Count == 0)
            {
                IsBusy = false;
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                observablePlaceList.Clear();
                if (listOfLocalPlaces != null)
                {
                    foreach (var place in listOfLocalPlaces)
                    {
                        if (place.PhotosBase64.Count > 0)
                            observablePlaceList.Add(place);
                    }
                }
            });

            IsBusy = false;
        }

        public ICommand RefreshCommand => new Command(() =>
        {
            StopLoading();
        });

        private StackLayout CreateMainLayout()
        {
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

            listView = new ListView
            {
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    var viewCell = new ViewCell();
                    var image = new Image
                    {
                        WidthRequest = 120,
                        HeightRequest = 120,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center
                    };
                    image.SetBinding(Image.SourceProperty, "MainPhoto");

                    var label1 = new Label
                    {
                        TextColor = (Color)Application.Current.Resources["MainAccentColor"],
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        FontAttributes = FontAttributes.Bold
                    };
                    label1.SetBinding(Label.TextProperty, "Name");

                    var label2 = new Label
                    {
                        TextColor = (Color)Application.Current.Resources["MainAccentColor"],
                        VerticalOptions = LayoutOptions.StartAndExpand
                    };
                    label2.SetBinding(Label.TextProperty, "Type");

                    var cellLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            image,
                            new StackLayout
                            {
                                Margin = new Thickness(0,10,0,10),
                                Children =
                                {
                                    label1, label2
                                }
                            }
                        }
                    };

                    viewCell.View = cellLayout;
                    return viewCell;
                }),
                ItemsSource = ObservablePlaceList,
                BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"],
                IsPullToRefreshEnabled = true,
                RefreshCommand = RefreshCommand
            };
            listView.SetBinding(ListView.IsRefreshingProperty, "IsBusy");

            listView.ItemSelected += (s, e) =>
            {
                var place = e.SelectedItem as Place;
                if (place != null)
                {
                    listView.SelectedItem = null;
                    CurrentPage.Navigation.PushAsync(new Views.Xaml.PlaceDetailsPage(place));
                }
            };

            var mainLayout = Utils.CreateItemOnItemLayout(listView, loadingStackLayout);
            return mainLayout;
        }
    }
}
