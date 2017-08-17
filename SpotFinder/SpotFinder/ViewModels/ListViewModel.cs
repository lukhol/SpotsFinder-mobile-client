using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Linq;

namespace SpotFinder.ViewModels
{
    public class ListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private INavigation Navigation { get; }
        private IPlaceRepository PlaceRepository { get; }
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
        private bool isBussy = false;

        public ListViewModel(INavigation navigation, IPlaceRepository placeRepository)
        {
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in ListViewModel");
            Navigation = navigation ?? throw new ArgumentNullException("navigation is null in ListViewModel");

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
            IsBussy = true;
        }

        public void StopLoading()
        {
            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();
            var placeList = reportManager.DownloadedPlaces;
            Device.BeginInvokeOnMainThread(() =>
            {
                observablePlaceList.Clear();
                if(placeList != null)
                {
                    foreach (var place in placeList)
                        observablePlaceList.Add(place);
                }
            });
            
            IsBussy = false;
        }

        public bool IsBussy
        {
            get => isBussy;
            set
            {
                isBussy = value;
                OnPropertyChanged();
            }
        }

        private StackLayout CreateMainLayout()
        {
            CurrentPage.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "criteriaIcon.png",
                Command = new Command(async () => { await Navigation.PushAsync(new CriteriaPage()); })
            });

            CurrentPage.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "plusIcon.png",
                Command = new Command(async () => { await Navigation.PushAsync(new AddingProcessPage()); }),
                
            });

            CurrentPage.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "refreshIcon.png",
                Command = new Command(() => { StopLoading(); })
            });

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
            loadingStackLayout.SetBinding(StackLayout.IsVisibleProperty, "IsBussy");

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
            };
            
            listView.ItemSelected += (s, e) =>
            {
                var place = e.SelectedItem as Place;
                if (place != null)
                {
                    listView.SelectedItem = null;
                    Navigation.PushAsync(new PlaceDetailsPage(place));
                }
            };

            var mainLayout = Utils.CreateItemOnItemLayout(listView, loadingStackLayout);
            return mainLayout;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
