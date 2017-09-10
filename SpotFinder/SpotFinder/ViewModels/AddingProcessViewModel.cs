using Microsoft.Practices.ServiceLocation;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SpotFinder.Core;
using SpotFinder.Core.Enums;
using SpotFinder.DataServices;
using SpotFinder.OwnControls;
using SpotFinder.Repositories;
using SpotFinder.Resx;
using SpotFinder.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class AddingProcessViewModel : BaseViewModel
    {
        private IPlaceService PlaceRepository { get; }
        private ILocalPlaceRepository LocalPlaceRepository { get; }
        private ContentPage CurrentPage { get; set; }
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];

        private StackLayout photoStackLayout = new StackLayout();

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

        public AddingProcessViewModel(IPlaceService placeRepository, ILocalPlaceRepository localPlaceRepository)
        {          
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
            ReportManager = ServiceLocator.Current.GetInstance<ReportManager>();

            ReportManager.CreateNewReport();

            CanGoBack = false;

            scrollView = new ScrollView
            {
                Content = CreateWaitingLayout()
            };

            CurrentPage.Content = scrollView;

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

            if(ReportManager.AddingPlace.PhotosAsImage.Count < 5)
            {
                var pickTypeResult = await CurrentPage.DisplayAlert("Where?", "Chose from where you want pick photo:", "Camera", "Gallery");
                Tuple<Image, string> imageTuple = null;

                if (pickTypeResult)
                    imageTuple = await GetPhotoAsync(GetPhotoType.Camera);
                else
                    imageTuple = await GetPhotoAsync(GetPhotoType.Gallery);

                if (imageTuple == null)
                    return;

                var image = (MyImage)imageTuple.Item1;
                image.Base64Representation = imageTuple.Item2;

                if(image != null)
                {
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => {
                        var img = (MyImage)s;

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await CurrentPage.DisplayAlert("Alert!", AppResources.AlertRemovePhoto, AppResources.AlertYes, AppResources.AlertNo);
                            if (result)
                            {
                                img.RemoveFromParent();
                                ReportManager.AddingPlace.PhotosAsImage.Remove(img);

                                if (ReportManager.AddingPlace.PhotosAsImage.Count == 0)
                                    ReportButton.IsVisible = false;

                                if (ReportManager.AddingPlace.PhotosAsImage.Count == 4)
                                    AddPhotoButton.IsVisible = true;
                            }
                        });
                    };
                    image.GestureRecognizers.Add(tapGestureRecognizer);

                    photoStackLayout.Children.Add(image);
                    ReportManager.AddingPlace.PhotosAsImage.Add(image);
                }

                if(ReportManager.AddingPlace.PhotosAsImage.Count == 5)
                    AddPhotoButton.IsVisible = false;
            }

            if (ReportManager.AddingPlace.PhotosAsImage.Count >= 1)
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
            ReportManager.AddingPlace.Location.Latitude = latitude;
            ReportManager.AddingPlace.Location.Longitude = longitude;
            ReportManager.AddingPlace.Description = Utils.FirstLetterToUpperCase(DescriptionEntry.Text);
            ReportManager.AddingPlace.Name = Utils.FirstLetterToUpperCase(NameEntry.Text);

            ReportManager.AddingPlace.Gap = booleanFieldsMap["Gap"].IsToggled;
            ReportManager.AddingPlace.Stairs = booleanFieldsMap["Stairs"].IsToggled;
            ReportManager.AddingPlace.Rail = booleanFieldsMap["Rail"].IsToggled;
            ReportManager.AddingPlace.Ledge = booleanFieldsMap["Ledge"].IsToggled;
            ReportManager.AddingPlace.Handrail = booleanFieldsMap["Handrail"].IsToggled;
            ReportManager.AddingPlace.Hubba = booleanFieldsMap["Hubba"].IsToggled;
            ReportManager.AddingPlace.Corners = booleanFieldsMap["Corners"].IsToggled;
            ReportManager.AddingPlace.Manualpad = booleanFieldsMap["Manualpad"].IsToggled;
            ReportManager.AddingPlace.Wallride = booleanFieldsMap["Wallride"].IsToggled;
            ReportManager.AddingPlace.Downhill = booleanFieldsMap["Downhill"].IsToggled;
            ReportManager.AddingPlace.OpenYourMind = booleanFieldsMap["OpenYourMind"].IsToggled;
            ReportManager.AddingPlace.Pyramid = booleanFieldsMap["Pyramid"].IsToggled;
            ReportManager.AddingPlace.Curb = booleanFieldsMap["Curb"].IsToggled;
            ReportManager.AddingPlace.Bank = booleanFieldsMap["Bank"].IsToggled;
            ReportManager.AddingPlace.Bowl = booleanFieldsMap["Bowl"].IsToggled;

            foreach(var img in ReportManager.AddingPlace.PhotosAsImage)
            {
                var myImg = (MyImage)img;
                if(!ReportManager.AddingPlace.PhotosBase64.Contains(myImg.Base64Representation))
                    ReportManager.AddingPlace.PhotosBase64.Add(myImg.Base64Representation);
            }

            //Spot type added runtime
            //Id missing there
            /*
            CurrentPage.Navigation.PushAsync(new PlaceDetailsPage(ReportManager.Place,
                new Command(() =>
                {
                    CurrentPage.Navigation.PushAsync(new LocateOnMapPage());
                }), AppResources.NextCommandTitle));
            */

            CurrentPage.Navigation.PushAsync(new LocateOnMapPage());
        });

        private async Task<Tuple<Image, string>> GetPhotoAsync(GetPhotoType getPhotoType)
        {
            await CrossMedia.Current.Initialize();

            MediaFile file = null;
            
            if(getPhotoType == GetPhotoType.Camera)
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return null;
                }

                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "SpotsFindePictures",
                    Name = "SpotsFinder.jpg",
                    PhotoSize = PhotoSize.Small,
                    SaveToAlbum = true
                });
            } 
            else if(getPhotoType == GetPhotoType.Gallery)
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    return null;
                }

                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    CompressionQuality = 0
                });
            }

            if (file == null)
                return null;

            var image = new MyImage
            {
                Margin = new Thickness(5,0,5,5),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            //Ten stream może być tworzyny w zły sposób. Nie wiem czy jest potrzeba pobierać go aż 2 razy.
            var stream = file.GetStream();
            image.Source = ImageSource.FromStream(() =>
            {
                return stream;
            });

            var streamTwo = file.GetStream();
            file.Dispose();
            var bytes = new byte[streamTwo.Length];
            await streamTwo.ReadAsync(bytes, 0, (int)streamTwo.Length);

            return new Tuple<Image, string>(image, Convert.ToBase64String(bytes));
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
            var types = new Dictionary<string, PlaceType>
            {
                {"Skatepark", PlaceType.Skatepark },
                {"Skatespot", PlaceType.Skatespot },
                {"DIY", PlaceType.DIY}
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
                ReportManager.AddingPlace.Type = types[picker.Items[picker.SelectedIndex]];
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

        private StackLayout CreateTipLayout()
        {
            return new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        TextColor = mainAccentColor,
                        Text = AppResources.AddingInformation,
                        Margin = new Thickness(12)
                    }
                }
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
                    CreateTipLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateEntryLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateBooleanFieldsLayout(),
                    Utils.CreateGridSeparator(12),
                    photoStackLayout,
                    AddPhotoButton,
                    ReportButton
                }
            };
            return layout;
        }

        private enum GetPhotoType
        {
            Camera, Gallery
        }
    }
}
