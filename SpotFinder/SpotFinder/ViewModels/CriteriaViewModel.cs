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

        public CriteriaViewModel(INavigation navigation)
        {
            Navigation = navigation;

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

            typeFieldsMap = new Dictionary<Core.Type, Switch>
            {
                {Core.Type.Skatepark, CreateParameterSwitch(Color.White) },
                {Core.Type.Skatespot, CreateParameterSwitch(Color.White) },
                {Core.Type.DIY, CreateParameterSwitch(Color.White) }
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
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        Text = AppResources.ObstaclesLabel,
                        Margin = new Thickness(5,0,5,0)
                    }
                }
            };

            foreach(var field in booleanFieldsMap)
            {
                var label = new Label
                {
                    TextColor = Color.White,
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
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                        Text = AppResources.TypeLabel,
                        Margin = new Thickness(5,0,5,0)
                    }
                }
            };

            foreach (var field in typeFieldsMap)
            {
                var label = new Label
                {
                    TextColor = Color.White,
                    Text = field.Key.ToString()
                };
                var horizontalLayout = Utils.CreateHorizontalStackLayout(field.Value, label);
                layout.Children.Add(horizontalLayout);
            }
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
                    Utils.CreateGridButton(SelectAllCommand, AppResources.SelectAllCommand),
                    Utils.CreateGridButton(FilterButtonCommand, AppResources.FilterLabel)
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

        public Command FilterButtonCommand => new Command(() =>
        {

        });
    }
}
