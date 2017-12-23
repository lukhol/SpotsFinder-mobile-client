using SpotFinder.Core;
using SpotFinder.Models.Core;
using SpotFinder.Redux.Actions;
using SpotFinder.Services;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using System;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceDetailsPage : NavContentPage
    {
        public PlaceDetailsPage()
        {
            InitializeComponent();
            PreapreListItemSelected();
            BindingContext = Unity.Instance.Resolve<PlaceDetailsViewModel>();
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