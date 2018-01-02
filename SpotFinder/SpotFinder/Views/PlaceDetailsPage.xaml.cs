using SpotFinder.Config;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceDetailsPage : NavContentPage
    {
        public PlaceDetailsPage()
        {
            InitializeComponent();
            PreapreListItemSelected();
            BindingContext = DIContainer.Instance.Resolve<PlaceDetailsViewModel>();
        }

        //Because obstacles are in listview.
        private void PreapreListItemSelected()
        {
            ObstacleListView.ItemSelected += (s, e) =>
            {
                ObstacleListView.SelectedItem = null;
            };
        }
    }
}