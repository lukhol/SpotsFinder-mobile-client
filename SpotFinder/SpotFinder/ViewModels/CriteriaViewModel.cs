using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.Core.Enums;
using SpotFinder.Resx;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class CriteriaViewModel : BaseViewModel
    {
        private IPlaceRepository PlaceRepository { get; }

        private Dictionary<string, Switch> booleanFieldsMap;
        private Dictionary<PlaceType, Switch> typeFieldsMap;
        private ContentPage CurrentPage { get; set; }
        private Entry CityEntry;
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];

        public CriteriaViewModel(IPlaceRepository placeRepository)
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
            if (CityEntry.Text == null)
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

            var repo = new RestAdressRepository();
            var position = await repo.GetPositionOfTheCity(CityEntry.Text, true);

            if (position == null)
            {
                await CurrentPage.DisplayAlert("Error", "Problem with position. Are you connected to the internet?", "Ok");
                return;
            }

            criteria.Location.Longitude = position.Longitude;
            criteria.Location.Latitude = position.Latitude;

            foreach (var item in typeFieldsMap)
            {
                if (item.Value.IsToggled)
                    criteria.Types.Add(item.Key);
            }

            if(criteria.Types.Count == 0)
            {
                await CurrentPage.DisplayAlert("Validation", "You have to choose at least one type of places", "Ok");
                return;
            }
                

            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();
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
                Margin = new Thickness(12),
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

        private StackLayout CreateCriteriaLayout()
        {
            var layout = new StackLayout
            {
                Children =
                {
                    CreateTypeLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateObstaclesLayout(),
                    Utils.CreateGridSeparator(12),
                    CreateCityEntryLayout(),
                    Utils.CreateDownSiteButton(SelectAllCommand, AppResources.SelectAllCommand, new Thickness(12 ,0 ,12, 12)),
                    Utils.CreateDownSiteButton(FilterButtonCommand, AppResources.FilterLabel, new Thickness(12 ,0 ,12, 12))
                }
            };

            return layout;
        }
    }
}
