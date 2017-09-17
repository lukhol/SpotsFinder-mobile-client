using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using System.Windows.Input;
using SpotFinder.Models.Core;
using SpotFinder.DataServices;
using SpotFinder.Repositories;

namespace SpotFinder.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        private IPlaceService PlaceRepository { get; }
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

        public ListViewModel(IPlaceService placeRepository)
        {
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in ListViewModel");

            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();

            reportManager.StartEvent += StartLoading;
            reportManager.StopEvent += StopLoading;
        }

        public void InjectPage(ContentPage contentPage)
        {
            CurrentPage = contentPage;
            CurrentPage.Content = CreateMainLayout();
        }

        public void StartLoading()
        {
            IsBusy = true;
        }

        public void StopLoading()
        {
            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();
            if(reportManager.DownloadedPlaces == null)
            {
                IsBusy = false;
                return;
            }
            var placeList = reportManager.DownloadedPlaces;
            Device.BeginInvokeOnMainThread(() =>
            {
                observablePlaceList.Clear();
                if(placeList != null)
                {
                    foreach (var place in placeList)
                    {
                        if(place.PhotosBase64.Count > 0)
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
                        Text = "Refreshing spots on list...",
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
                                Margin = new Thickness(5,10,5,10),
                                Children =
                                {
                                    label1, label2
                                }
                            }
                        },
                        Margin = new Thickness(0,5,0,5)
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
                    //CurrentPage.Navigation.PushAsync(new PlaceDetailsPage(place));
                    CurrentPage.Navigation.PushAsync(new Views.Xaml.PlaceDetailsPage(place.Id));
                }
            };

            var mainLayout = Utils.CreateItemOnItemLayout(listView, loadingStackLayout);
            return mainLayout;
        }
    }
}
