using SpotFinder.Core;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private ContentPage CurrentPage;
        private Color mainAccentColor = (Color)Application.Current.Resources["MainAccentColor"];
        private Label distanceLabel;

        private double distance;
        public double Distance
        {
            get => distance;
            set
            {
                distance = value;
                distanceLabel.Text = "Ustal zasięg wyszukiwania: " + ((int)distance).ToString() + " km";
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
            Application.Current.Properties["MainDistance"] = distance;

            await Application.Current.SavePropertiesAsync();

            await CurrentPage.Navigation.PopAsync();
        }

        public SettingsViewModel()
        {

        }

        public void InjectPage(ContentPage contentPage)
        {
            CurrentPage = contentPage;
            CurrentPage.Content = CreateMainLayout();
        }

        public void CheckProperties()
        {
            if (Application.Current.Properties.ContainsKey("MainCity"))
                City = Application.Current.Properties["MainCity"] as string;

            if (Application.Current.Properties.ContainsKey("MainDistance"))
                Distance = (double)Application.Current.Properties["MainDistance"];     
        }

        private StackLayout CreateMainLayout()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];
            var layout = new StackLayout();

            var cityLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
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

            distanceLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                TextColor = mainAccentColor,
                Text = "Ustal zasięg wyszukiwania: 1 km",
                Margin = new Thickness(12, 0, 12, 12)
            };

            var distanceSlider = new Slider
            {
                Maximum = 50,
                Minimum = 1,
                Value = 5
            };
            distanceSlider.SetBinding(Slider.ValueProperty, "Distance");

            var saveButton = Utils.CreateDownSiteButton(SaveCommand, "Zapisz", new Thickness(12));
            saveButton.VerticalOptions = LayoutOptions.EndAndExpand;

            layout.Children.Add(cityLabel);
            layout.Children.Add(cityEntry);
            layout.Children.Add(distanceLabel);
            layout.Children.Add(distanceSlider);
            layout.Children.Add(saveButton);

            return layout;
        }
    }
}
