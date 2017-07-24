using SpotFinder.Core;
using SpotFinder.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class AddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation Navigation;
        private ContentPage CurrentPage { get; set; }

        public AddViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public void InjectPage(ContentPage contentPage, string pageTitle)
        {
            CurrentPage = contentPage;
            CurrentPage.Title = pageTitle;
            CurrentPage.Content = CreateAddLayout();
        }

        private StackLayout CreateAddLayout()
        {
            CurrentPage.BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];

            var addButton = Utils.CreateGridButton(StartAddingCommand, "Add place");
            addButton.VerticalOptions = LayoutOptions.CenterAndExpand;
            addButton.HorizontalOptions = LayoutOptions.CenterAndExpand;
            var layout = new StackLayout
            {
                Children =
                {
                    addButton
                }
            };
            return layout;
        }

        public Command StartAddingCommand => new Command(() =>
        {
            Navigation.PushAsync(new AddingProcessPage());
        });

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}