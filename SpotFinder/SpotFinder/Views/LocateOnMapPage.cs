using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SpotFinder.Views
{
    public class LocateOnMapPage : ContentPage
    {
        public LocateOnMapPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Title = "Locate on map page";
            var locateOnMapViewModel = ServiceLocator.Current.GetInstance<LocateOnMapViewModel>();
            locateOnMapViewModel.InjectPage(this);
        }
    }
}