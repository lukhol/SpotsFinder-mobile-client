using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Plugin.Geolocator;
using Plugin.Media;
using SpotFinder.Core;
using SpotFinder.OwnControls;
using SpotFinder.Resx;
using SpotFinder.Views;
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
        private IPlaceRepository PlaceRepository { get; }
        private ILocalPlaceRepository LocalPlaceRepository { get; }
        private ContentPage CurrentPage { get; set; }
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];

        private double latitude;
        private double longitude;
        private bool canGoBack;

        private ScrollView scrollView;
        private ReportManager ReportManager;

        private Button AddPhotoButton;
        private Button ReportButton;

        private Entry DescriptionEntry;
        private Entry NameEntry;

        private Picker PlaceTypePicker; //Runtime added to place.

        Dictionary<string, Switch> booleanFieldsMap;

        public bool CanGoBack
        {
            get => canGoBack;
            set
            {
                canGoBack = value;
                OnPropertyChanged();
            }
        }

        public AddingProcessViewModel(INavigation navigation, IPlaceRepository placeRepository, ILocalPlaceRepository localPlaceRepository)
        {
            //Jeżeli placeRepository jest null to rzuci się wyjątek?
            Navigation = navigation ?? throw new ArgumentNullException("navigation is null in AddingProcessViewModel");            
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in AddingProcessViewModel");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in AddingProcessViewModel");

            booleanFieldsMap = new Dictionary<string, Switch>
            {
                {"Gap", CreateParameterSwitch(mainAccentColor) },
                {"Stairs", CreateParameterSwitch(mainAccentColor) },
                {"Rail", CreateParameterSwitch(mainAccentColor) },
                {"Ledge", CreateParameterSwitch(mainAccentColor) },
                {"Handrail", CreateParameterSwitch(mainAccentColor) },
                {"Hubba", CreateParameterSwitch(mainAccentColor) },
                {"Corners", CreateParameterSwitch(mainAccentColor) },
                {"Manualpad", CreateParameterSwitch(mainAccentColor) },
                {"Wallride", CreateParameterSwitch(mainAccentColor) },
                {"Downhill", CreateParameterSwitch(mainAccentColor) },
                {"OpenYourMind", CreateParameterSwitch(mainAccentColor) },
                {"Pyramid", CreateParameterSwitch(mainAccentColor) },
                {"Curb", CreateParameterSwitch(mainAccentColor) },
                {"Bank", CreateParameterSwitch(mainAccentColor) },
                {"Bowl", CreateParameterSwitch(mainAccentColor) }
            };
        }

        public async void InjectPageAsync(ContentPage contentPage, string pageTitle)
        {
            CurrentPage = contentPage;
            CurrentPage.Title = pageTitle;

            await StartAsync();
        }

        //Start
        public async Task StartAsync()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];

            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            ReportManager = (ReportManager)serviceLocator.GetService(typeof(ReportManager));

            ReportManager.CreateNewReport();

            CanGoBack = false;
            var currPage = (ContentPage)Navigation.NavigationStack[Navigation.NavigationStack.Count - 1];

            scrollView = new ScrollView
            {
                Content = CreateWaitingLayout()
            };

            currPage.Content = scrollView;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(3));
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

            scrollView.Content = CreateAddingLayout();
            CanGoBack = true;
        }

        public Command AddPhotoCommand  => new Command(async () => 
        {
            var mainLayout = (StackLayout)scrollView.Content;

            if(ReportManager.Place.PhotosAsImage.Count < 5)
            {
                //To muszę sobie opisać komentarzem:
                var insertAtIndex = mainLayout.Children.Count - 2;
                var imageTuple = await GetPhotoAsync();

                if (imageTuple == null)
                    return;

                var image = (MyImage)imageTuple.Item1;
                image.Base64Representation = imageTuple.Item2;

                if(image != null)
                {
                    //Usuwanie po kliknięciu - należałoby dodać komunikat
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => {
                        var img = (MyImage)s;

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var currPage = Navigation.NavigationStack[Navigation.NavigationStack.Count() - 1];
                            var result = await currPage.DisplayAlert("Alert!", AppResources.AlertRemovePhoto, AppResources.AlertYes, AppResources.AlertNo);
                            if (result)
                            {
                                img.RemoveFromParent();
                                ReportManager.Place.PhotosAsImage.Remove(img);

                                if (ReportManager.Place.PhotosAsImage.Count == 0)
                                    ReportButton.IsVisible = false;

                                if (ReportManager.Place.PhotosAsImage.Count == 4)
                                    AddPhotoButton.IsVisible = true;
                            }
                        });

                        
                    };
                    image.GestureRecognizers.Add(tapGestureRecognizer);

                    mainLayout.Children.Insert(insertAtIndex, image);
                    ReportManager.Place.PhotosAsImage.Add(image);
                }

                if(ReportManager.Place.PhotosAsImage.Count == 5)
                    AddPhotoButton.IsVisible = false;
            }

            if (ReportManager.Place.PhotosAsImage.Count >= 1)
                ReportButton.IsVisible = true;
            else
                ReportButton.IsVisible = false;
        });

        public Command ReportCommand => new Command(() =>
        {
            var message = string.Empty;

            if (DescriptionEntry.Text == null || NameEntry.Text == null)
                message += "Name and description must be provided.\n";

            if (NameEntry.Text != null && NameEntry.Text.Length < 5)
                message += "Name have to be longer than 5.\n";

            if (DescriptionEntry.Text != null && DescriptionEntry.Text.Length < 5)
                message += "Description have to be longer than 5.";

            if (!string.IsNullOrEmpty(message))
            {
                CurrentPage.DisplayAlert("Validation", message, "Ok");
                return;
            }

            //Tutaj cała aktualizacja
            ReportManager.Place.Location.Latitude = latitude;
            ReportManager.Place.Location.Longitude = longitude;
            ReportManager.Place.Description = Utils.FirstLetterToUpperCase(DescriptionEntry.Text);
            ReportManager.Place.Name = Utils.FirstLetterToUpperCase(NameEntry.Text);

            ReportManager.Place.Gap = booleanFieldsMap["Gap"].IsToggled;
            ReportManager.Place.Stairs = booleanFieldsMap["Stairs"].IsToggled;
            ReportManager.Place.Rail = booleanFieldsMap["Rail"].IsToggled;
            ReportManager.Place.Ledge = booleanFieldsMap["Ledge"].IsToggled;
            ReportManager.Place.Handrail = booleanFieldsMap["Handrail"].IsToggled;
            ReportManager.Place.Hubba = booleanFieldsMap["Hubba"].IsToggled;
            ReportManager.Place.Corners = booleanFieldsMap["Corners"].IsToggled;
            ReportManager.Place.Manualpad = booleanFieldsMap["Manualpad"].IsToggled;
            ReportManager.Place.Wallride = booleanFieldsMap["Wallride"].IsToggled;
            ReportManager.Place.Downhill = booleanFieldsMap["Downhill"].IsToggled;
            ReportManager.Place.OpenYourMind = booleanFieldsMap["OpenYourMind"].IsToggled;
            ReportManager.Place.Pyramid = booleanFieldsMap["Pyramid"].IsToggled;
            ReportManager.Place.Curb = booleanFieldsMap["Curb"].IsToggled;
            ReportManager.Place.Bank = booleanFieldsMap["Bank"].IsToggled;
            ReportManager.Place.Bowl = booleanFieldsMap["Bowl"].IsToggled;

            foreach(var img in ReportManager.Place.PhotosAsImage)
            {
                var myImg = (MyImage)img;
                if(!ReportManager.Place.PhotosBase64.Contains(myImg.Base64Representation))
                    ReportManager.Place.PhotosBase64.Add(myImg.Base64Representation);
            }

            //Spot type added runtime
            //Id missing there

            Navigation.PushAsync(new PlaceDetailsPage(ReportManager.Place,
                new Command(() =>
                {
                    Navigation.PushAsync(new LocateOnMapPage());
                }), AppResources.NextCommandTitle));
        });

        private async Task<Tuple<Image, string>> GetPhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "SpotsFindePictures",
                Name = "SpotsFinder.jpg",
                //PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                //CustomPhotoSize = 40,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                SaveToAlbum = true
            });

            if (file == null)
                return null;

            var image = new MyImage
            {
                Margin = new Thickness(5,0,5,5),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            //Tutaj refactor itd
            var streamTwo = file.GetStream();
            var bytes = new byte[streamTwo.Length];
            await streamTwo.ReadAsync(bytes, 0, (int)streamTwo.Length);
            string base64 = Convert.ToBase64String(bytes);

            return new Tuple<Image, string>(image, base64);
        }

        //=================================================================================
        //================================== LAYOUT =======================================
        //=================================================================================

        private Button CreateAddPhotoButton()
        {
            var button = new Button
            {
                Text = AppResources.AddPhotoButton,
                VerticalOptions = LayoutOptions.Start
            };
            button.Command = AddPhotoCommand;
            return button;
        }

        private Button CreateReportButton()
        {
            var button = new Button
            {
                Text = AppResources.NextCommandTitle
            };
            button.Command = ReportCommand;
            return button;
        }

        private Picker CreatePlaceTypePicker()
        {
            var types = new Dictionary<string, Core.Type>
            {
                {"Skatepark", Core.Type.Skatepark },
                {"Skatespot", Core.Type.Skatespot },
                {"DIY", Core.Type.DIY}
            };

            var picker = new Picker
            {
                Title = AppResources.SpotTypePlaceholder,
                TextColor = Color.Black,
                //BackgroundColor = new Color(128,128,128,50),
                Margin = new Thickness(5,0,5,0)
            };

            foreach (string type in types.Keys)
            {
                picker.Items.Add(type);
            }

            picker.SelectedIndexChanged += (sender, args) =>
            {
                ReportManager.Place.Type = types[picker.Items[picker.SelectedIndex]];
            };

            return picker;
        }

        private StackLayout CreateWaitingLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = AppResources.AcquiringLocationLabel,
                        TextColor = mainAccentColor,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand
                    },
                    new ActivityIndicator
                    {
                        IsRunning = true,
                        Color = mainAccentColor,
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
                        Text = "Location:",
                        TextColor = mainAccentColor,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand
                    },
                    new Label
                    {
                        Text = "Latitude: " + latitude.ToString(),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        TextColor = mainAccentColor
                    },
                    new Label
                    {
                        Text = "Longitude: " + longitude.ToString(),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        TextColor = mainAccentColor
                    }
                }
            };
            return layout;
        }

        private StackLayout CreateEntryLayout()
        {
            NameEntry = new Entry
            {
                //BackgroundColor = new Color(128, 128, 128, 50),
                PlaceholderColor = Color.Gray,
                Placeholder = AppResources.NamePlaceholder,
                Margin = new Thickness(5,0,5,0),
            };
            DescriptionEntry = new Entry
            {
                //BackgroundColor = new Color(128, 128, 128, 50),
                PlaceholderColor = Color.Gray,
                Placeholder = AppResources.DescriptionPlaceholder,
                Margin = new Thickness(5, 0, 5, 0)
            };
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        TextColor = mainAccentColor,
                        FontAttributes = FontAttributes.Bold,
                        Text = AppResources.NamePlaceholder + ":",
                        Margin = new Thickness(5,0,5,0)
                    },
                    NameEntry,
                    new Label
                    {
                        TextColor = mainAccentColor,
                        FontAttributes = FontAttributes.Bold,
                        Text = AppResources.DescriptionPlaceholder + ":",
                        Margin = new Thickness(5,0,5,0)
                    },
                    DescriptionEntry,
                    new Label
                    {
                        TextColor = mainAccentColor,
                        Text = AppResources.TypeLabel,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(5,0,5,0)
                    },
                    CreatePlaceTypePicker()
                },
                Margin = new Thickness(12)
            };
            return layout;
        }

        private StackLayout CreateBooleanFieldsLayout()
        {
            var layout = new StackLayout
            {
                Margin = new Thickness(12)
            };
            foreach (var item in booleanFieldsMap)
            {
                item.Value.IsToggled = false;
                var horizontalLayout = Utils.CreateHorizontalStackLayout(item.Value,
                    new Label
                    {
                        VerticalOptions = LayoutOptions.Start,
                        TextColor = mainAccentColor,
                        Text = item.Key
                    }
                );
                layout.Children.Add(horizontalLayout);
            }
            return layout;
        }

        private Switch CreateParameterSwitch(Color color)
        {
            return new Switch
            {
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(5,0,5,0)
            };
        }

        private StackLayout CreateAddingLayout()
        {
            AddPhotoButton = Utils.CreateDownSiteButton(AddPhotoCommand, AppResources.AddPhotoButton, new Thickness(12));
            ReportButton = Utils.CreateDownSiteButton(ReportCommand, AppResources.NextCommandTitle, new Thickness(12, 0, 12, 12));
            ReportButton.IsVisible = false;
            var layout = new StackLayout
            {
                Children =
                {
                    CreateEntryLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateBooleanFieldsLayout(),
                    Utils.CreateGridSeparator(12),
                    AddPhotoButton,
                    ReportButton
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
