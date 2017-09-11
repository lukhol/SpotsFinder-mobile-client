﻿using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.Core.Enums;
using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.Resx;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class CriteriaViewModel : BaseViewModel
    {
        private IPlaceService PlaceRepository { get; }

        private Dictionary<string, Switch> booleanFieldsMap;
        private Dictionary<PlaceType, Switch> typeFieldsMap;
        private ContentPage CurrentPage { get; set; }
        private StackLayout EntryLayout;
        private Entry CityEntry;
        private Label distanceInfoLabel;
        private Slider DistanceSlider;
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];

        private double distance;
        public double Distance
        {
            get => distance;
            set
            {
                distance = value;
                distanceInfoLabel.Text = "Ustal zasięg wyszukiwania: " + ((int)distance).ToString() + " km";
                OnPropertyChanged();
            }
        }

        private bool usePhoneLocation;
        public bool UsePhoneLocation
        {
            get => usePhoneLocation;
            set
            {
                CityEntry.IsEnabled = !value;
                EntryLayout.IsEnabled = !value;

                usePhoneLocation = value;
                OnPropertyChanged();
            }
        }

        public CriteriaViewModel(IPlaceService placeRepository)
        {
            PlaceRepository = placeRepository;

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

            typeFieldsMap = new Dictionary<PlaceType, Switch>
            {
                {PlaceType.Skatepark, CreateParameterSwitch(mainAccentColor) },
                {PlaceType.Skatespot, CreateParameterSwitch(mainAccentColor) },
                {PlaceType.DIY, CreateParameterSwitch(mainAccentColor) }
            };
        }

        public void InjectPage(ContentPage contentPage)
        {
            CurrentPage = contentPage;
            CurrentPage.Content = new ScrollView
            {
                Content = CreateCriteriaLayout()
            };
        }

        public Command SelectAllCommand => new Command(() =>
        {
            foreach (var item in booleanFieldsMap)
            {
                item.Value.IsToggled = true;
            }
        });

        public Command FilterButtonCommand => new Command(async () =>
        {
            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();

            if (CityEntry.Text == null && CityEntry.IsEnabled == true)
                return;

            var criteria = new Criteria();

            criteria.Gap = booleanFieldsMap["Gap"].IsToggled;
            criteria.Stairs = booleanFieldsMap["Stairs"].IsToggled;
            criteria.Rail = booleanFieldsMap["Rail"].IsToggled;
            criteria.Ledge = booleanFieldsMap["Ledge"].IsToggled;
            criteria.Handrail = booleanFieldsMap["Handrail"].IsToggled;
            criteria.Hubba = booleanFieldsMap["Hubba"].IsToggled;
            criteria.Corners = booleanFieldsMap["Corners"].IsToggled;
            criteria.Manualpad = booleanFieldsMap["Manualpad"].IsToggled;
            criteria.Wallride = booleanFieldsMap["Wallride"].IsToggled;
            criteria.Downhill = booleanFieldsMap["Downhill"].IsToggled;
            criteria.OpenYourMind = booleanFieldsMap["OpenYourMind"].IsToggled;
            criteria.Pyramid = booleanFieldsMap["Pyramid"].IsToggled;
            criteria.Curb = booleanFieldsMap["Curb"].IsToggled;
            criteria.Bank = booleanFieldsMap["Bank"].IsToggled;
            criteria.Bowl = booleanFieldsMap["Bowl"].IsToggled;

            if (CityEntry.IsEnabled)
            {
                criteria.Location.City = CityEntry.Text;
                criteria.Location.Latitude = null;
                criteria.Location.Longitude = null;
            }
            else
            {
                if (reportManager.Location == null)
                    return;

                criteria.Location.Longitude = (double)reportManager.Location.Longitude;
                criteria.Location.Latitude = (double)reportManager.Location.Latitude;
                criteria.Location.City = null;
            }
            
            criteria.Distance = (int)Distance;

            foreach (var item in typeFieldsMap)
            {
                if (item.Value.IsToggled)
                    criteria.Type.Add(item.Key);
            }

            if(criteria.Type.Count == 0)
            {
                await CurrentPage.DisplayAlert("Validation", "You have to choose at least one type of places", "Ok");
                return;
            }
                
            reportManager.Criteria = criteria;

            await CurrentPage.Navigation.PopAsync();
        });

        private Switch CreateParameterSwitch(Color color)
        {
            return new Switch
            {
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(5, 0, 5, 0)
            };
        }

        private StackLayout CreateObstaclesLayout()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];
            CurrentPage.Title = AppResources.FilterLabel;

            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        TextColor = mainAccentColor,
                        FontAttributes = FontAttributes.Bold,
                        Text = AppResources.ObstaclesLabel,
                        Margin = new Thickness(5,0,5,0)
                    }
                },
                Margin = new Thickness(12)
            };

            foreach(var field in booleanFieldsMap)
            {
                var label = new Label
                {
                    TextColor = mainAccentColor,
                    Text = field.Key
                };
                var horizontalLayout = Utils.CreateHorizontalStackLayout(field.Value, label);
                layout.Children.Add(horizontalLayout);
            }
            return layout;
        }

        private StackLayout CreateTypeLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        TextColor = mainAccentColor,
                        FontAttributes = FontAttributes.Bold,
                        Text = AppResources.TypeLabel,
                        Margin = new Thickness(5,0,5,0)
                    }
                },
                Margin = new Thickness(12)
            };

            foreach (var field in typeFieldsMap)
            {
                var label = new Label
                {
                    TextColor = mainAccentColor,
                    Text = field.Key.ToString()
                };
                var horizontalLayout = Utils.CreateHorizontalStackLayout(field.Value, label);
                layout.Children.Add(horizontalLayout);
            }
            return layout;
        }

        private StackLayout CreateCityEntryLayout()
        {
            CityEntry = new Entry
            {
                //BackgroundColor = Color.FromRgba(128, 128, 128, 220),
                PlaceholderColor = Color.Black,
                Placeholder = AppResources.CityPlaceholder,
                Margin = new Thickness(12,12,12,12)
            };

            var layout = new StackLayout
            {
                Children =
                {
                    CityEntry
                }
            };

            return layout;
        }

        private  StackLayout CreateMyLocationLayout()
        {
            var infoLabel = new Label
            {
                Text = "Czy chcesz użyć swojej lokalizacji?",
                TextColor = mainAccentColor,
                Margin = new Thickness(12, 12, 12, 0)
            };
            var usePhoneLocationSwitch = CreateParameterSwitch(mainAccentColor);
            usePhoneLocationSwitch.Margin = new Thickness(0, 8, 0, 0);
            usePhoneLocationSwitch.SetBinding(Switch.IsToggledProperty, "UsePhoneLocation");

            return Utils.CreateHorizontalStackLayout(infoLabel, usePhoneLocationSwitch);
        }

        private StackLayout CreateDistanceLayout()
        {
            distanceInfoLabel = new Label
            {
                Text = "Ustal zasięg wyszukiwania: 1 km",
                TextColor = mainAccentColor,
                Margin = new Thickness(12,12,12,0)
            };

            DistanceSlider = new Slider
            {
                Maximum = 50,
                Minimum = 1,
                Value = 5,
                Margin = new Thickness(0)
            };
            DistanceSlider.SetBinding(Slider.ValueProperty, "Distance");

            var layout = new StackLayout
            {
                Children =
                {
                    distanceInfoLabel,
                    DistanceSlider
                }
            };

            return layout;
        }

        private StackLayout CreateCriteriaLayout()
        {
            EntryLayout = CreateCityEntryLayout();
            var layout = new StackLayout
            {
                Children =
                {
                    CreateTypeLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateObstaclesLayout(),
                    //Utils.CreateDownSiteButton(SelectAllCommand, AppResources.SelectAllCommand, new Thickness(12 ,0 ,12, 12)),
                    Utils.CreateGridSeparator(12),
                    //Utils.CreateGridSeparator(12),
                    CreateDistanceLayout(),
                    EntryLayout,
                    Utils.CreateGridSeparator(12),
                    CreateMyLocationLayout(),
                    //Utils.CreateGridSeparator(12),
                    Utils.CreateDownSiteButton(FilterButtonCommand, AppResources.FilterLabel, new Thickness(12 ,12 ,12, 12))
                }
            };

            return layout;
        }
    }
}
