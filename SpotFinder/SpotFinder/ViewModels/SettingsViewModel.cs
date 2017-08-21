using SpotFinder.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private ContentPage CurrentPage;
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];
        private Label distanceValueLabel;

        private int distance;
        public int Distance
        {
            get => distance;
            set
            {
                distance = value;
                distanceValueLabel.Text = value.ToString() + " km";
                OnPropertyChanged();
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand => new Command(SaveSettings);

        public async void SaveSettings()
        {
            Application.Current.Properties["MainCity"] = city;
            Application.Current.Properties["Distance"] = distance;

            await Application.Current.SavePropertiesAsync();

            await CurrentPage.Navigation.PopAsync();
        }

        public SettingsViewModel()
        {
            if (Application.Current.Properties.ContainsKey("MainCity"))
                City = Application.Current.Properties["MainCity"] as string;

            if (Application.Current.Properties.ContainsKey("Distance"))
                distance = (int)Application.Current.Properties["Distance"];
        }

        public void InjectPage(ContentPage contentPage)
        {
            CurrentPage = contentPage;
            CurrentPage.Content = CreateMainLayout();
        }

        private StackLayout CreateMainLayout()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];
            var layout = new StackLayout();

            var cityLabel = new Label
            {
                TextColor = mainAccentColor,
                Text = "Wpisz swoje miasto główne:",
                Margin = new Thickness(12,12,12,12)
            };

            var cityEntry = new Entry
            {
                Placeholder = "City",
                Margin = new Thickness(12,0,12,12)
            };
            cityEntry.SetBinding(Entry.TextProperty, "City");

            var distanceLabel = new Label
            {
                TextColor = mainAccentColor,
                Text = "Wybierz odległość:",
                Margin = new Thickness(12, 0, 12, 12)
            };

            var distanceSlider = new Slider
            {
                Minimum = 0,
                Maximum = 30,
                Margin = new Thickness(12)
            };
            distanceSlider.SetBinding(Slider.ValueProperty, "Distance");

            distanceValueLabel = new Label
            {
                TextColor = mainAccentColor,
                Text = "0 km",
                Margin = new Thickness(12, 0, 10, 12)
            };

            var saveButton = Utils.CreateDownSiteButton(SaveCommand, "Zapisz", new Thickness(12));
            saveButton.VerticalOptions = LayoutOptions.EndAndExpand;

            layout.Children.Add(cityLabel);
            layout.Children.Add(cityEntry);
            layout.Children.Add(distanceLabel);
            layout.Children.Add(distanceSlider);
            layout.Children.Add(distanceValueLabel);
            layout.Children.Add(saveButton);

            return layout;
        }
    }
}
