using SpotFinder.Core;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceDetailsPage : ContentPage
    {
        private Place place;
        private Command command;
        private string message;

        public PlaceDetailsPage(Place place, Command command = null, string message = null)
        {
            this.place = place;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            if(command != null)
            {
                this.command = command;
                this.message = message;
            }
        }

        protected override void OnAppearing()
        {
            var placeDetailsViewModel = new PlaceDetailsViewModel(Navigation);

            BindingContext = placeDetailsViewModel;
            if (command != null)
            {
                placeDetailsViewModel.Initialize(place, command, message);

            }
            else
            {
                placeDetailsViewModel.Initialize(place);
            }

            BindingContext = placeDetailsViewModel;

            base.OnAppearing();
        }
    }
}