using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.Core;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
		public MapPage ()
		{
			InitializeComponent();
            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var mapPageViewModel = (MapPageViewModel)serviceLocator.GetService(typeof(MapPageViewModel));
            BindingContext = mapPageViewModel;
        }

        protected override void OnAppearing()
        {
            var mapPageViewModel = (MapPageViewModel)BindingContext;
            mapPageViewModel.ShowMap();
            base.OnAppearing();
        }
    }
}