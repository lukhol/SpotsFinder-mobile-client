using SpotFinder.Core;
using SpotFinder.Resx;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class CriteriaViewModel
    {
        private INavigation Navigation { get; }

        private Dictionary<string, Switch> booleanFieldsMap;
        private Dictionary<Core.Type, Switch> typeFieldsMap;
        private ContentPage CurrentPage { get; set; }
        private Entry CityEntry;
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];

        public CriteriaViewModel(INavigation navigation)
        {
            Navigation = navigation;

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

            typeFieldsMap = new Dictionary<Core.Type, Switch>
            {
                {Core.Type.Skatepark, CreateParameterSwitch(mainAccentColor) },
                {Core.Type.Skatespot, CreateParameterSwitch(mainAccentColor) },
                {Core.Type.DIY, CreateParameterSwitch(mainAccentColor) }
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

        public Command SelectAllCommand => new Command(() => 
        {
            foreach(var item in booleanFieldsMap)
            {
                item.Value.IsToggled = true;
            }
        });

        public Command FilterButtonCommand => new Command(async () =>
        {
            if (CityEntry.Text == null)
                return;

            var repo = new RestAdressRepository();
            var position = await repo.GetPositionOfTheCity(CityEntry.Text, true);
            if (position != null)
                await CurrentPage.DisplayAlert("Message", position.Longitude.ToString() + "\n" + position.Latitude.ToString(), "Ok");
            else
                await CurrentPage.DisplayAlert("Message", "Problem with position", "Ok");
        });
    }
}
