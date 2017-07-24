using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Plugin.Geolocator;
using Plugin.Media;
using SpotFinder.Core;
using SpotFinder.OwnControls;
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

        private double latitude;
        private double longitude;
        private bool canGoBack;

        private ScrollView scrollView;
        private ReportManager ReportManager;

        private StackLayout AddPhotoButton;
        private StackLayout ReportButton;

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
                {"Gap", CreateParameterSwitch(Color.White) },
                {"Stairs", CreateParameterSwitch(Color.White) },
                {"Rail", CreateParameterSwitch(Color.White) },
                {"Ledge", CreateParameterSwitch(Color.White) },
                {"Handrail", CreateParameterSwitch(Color.White) },
                {"Corners", CreateParameterSwitch(Color.White) },
                {"Manualpad", CreateParameterSwitch(Color.White) },
                {"Wallride", CreateParameterSwitch(Color.White) },
                {"Downhill", CreateParameterSwitch(Color.White) },
                {"OpenYourMind", CreateParameterSwitch(Color.White) },
                {"Pyramid", CreateParameterSwitch(Color.White) },
                {"Curb", CreateParameterSwitch(Color.White) },
                {"Bank", CreateParameterSwitch(Color.White) },
                {"Bowl", CreateParameterSwitch(Color.White) }
            };
        }

        //Start
        public async Task StartAsync()
        {
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
                            var result = await currPage.DisplayAlert("Alert!", "Do you want remove this photo?", "Yes", "No");
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
            //Tutaj cała aktualizacja
            ReportManager.Place.Location.Latitude = latitude;
            ReportManager.Place.Location.Longitude = longitude;
            ReportManager.Place.Description = DescriptionEntry.Text;
            ReportManager.Place.Name = NameEntry.Text;

            ReportManager.Place.Gap = booleanFieldsMap["Gap"].IsToggled;
            ReportManager.Place.Stairs = booleanFieldsMap["Stairs"].IsToggled;
            ReportManager.Place.Rail = booleanFieldsMap["Rail"].IsToggled;
            ReportManager.Place.Ledge = booleanFieldsMap["Ledge"].IsToggled;
            ReportManager.Place.Handrail = booleanFieldsMap["Handrail"].IsToggled;
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
                    PlaceRepository.Send(ReportManager.Place);
                    Navigation.PopToRootAsync();
                }), "Confirm"));
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
            string base64 = System.Convert.ToBase64String(bytes);

            return new Tuple<Image, string>(image, base64);
        }

        //=================================================================================
        //================================== LAYOUT =======================================
        //=================================================================================

        private Button CreateAddPhotoButton()
        {
            var button = new Button
            {
                Text = "Add photo",
                VerticalOptions = LayoutOptions.Start
            };
            button.Command = AddPhotoCommand;
            return button;
        }

        private Button CreateReportButton()
        {
            var button = new Button
            {
                Text = "Next"
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
                Title = "Spot type",
                TextColor = Color.Black,
                BackgroundColor = new Color(128,128,128,50),
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
                        Text = "Acquiring location",
                        TextColor = Color.White,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand
                    },
                    new ActivityIndicator
                    {
                        IsRunning = true,
                        Color = Color.White,
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
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand
                    },
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

        private StackLayout CreateEntryLayout()
        {
            NameEntry = new Entry
            {
                BackgroundColor = new Color(128, 128, 128, 50),
                PlaceholderColor = Color.Black,
                Placeholder = "Name",
                Margin = new Thickness(5,0,5,0)
            };
            DescriptionEntry = new Entry
            {
                BackgroundColor = new Color(128, 128, 128, 50),
                PlaceholderColor = Color.Black,
                Placeholder = "Description",
                Margin = new Thickness(5, 0, 5, 0)
            };
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        Text = "Name:",
                        Margin = new Thickness(5,0,5,0)
                    },
                    NameEntry,
                    new Label
                    {
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        Text = "Description:",
                        Margin = new Thickness(5,0,5,0)
                    },
                    DescriptionEntry,
                    new Label
                    {
                        TextColor = Color.White,
                        Text = "Type:",
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(5,0,5,0)
                    },
                    CreatePlaceTypePicker()
                }
            };
            return layout;
        }

        private StackLayout CreateBooleanFieldsLayout()
        {
            var layout = new StackLayout();
            foreach (var item in booleanFieldsMap)
            {
                item.Value.IsToggled = false;
                var horizontalLayout = Utils.CreateHorizontalStackLayout(item.Value,
                    new Label
                    {
                        VerticalOptions = LayoutOptions.Start,
                        TextColor = Color.White,
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
            AddPhotoButton = Utils.CreateGridButton(AddPhotoCommand, "Add photo", 5);
            ReportButton = Utils.CreateGridButton(ReportCommand, "Next", 5);
            ReportButton.IsVisible = false;
            var layout = new StackLayout
            {
                Children =
                {
                    CreateLocationLabels(),
                    Utils.CreateGridSeparator(5),
                    CreateEntryLayout(),
                    Utils.CreateGridSeparator(5),
                    CreateBooleanFieldsLayout(),
                    Utils.CreateGridSeparator(5),
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
