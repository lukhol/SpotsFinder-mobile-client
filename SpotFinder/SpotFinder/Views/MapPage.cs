﻿using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;


using Xamarin.Forms;

namespace SpotFinder.Views
{
	public class MapPage : ContentPage
	{
		public MapPage()
		{
            CreateToolbarItems();
            var mapPageViewModel = ServiceLocator.Current.GetInstance<MapPageViewModel>();
            mapPageViewModel.InjectPage(this, "Mapa");
            BindingContext = mapPageViewModel;
        }

        private void CreateToolbarItems()
        {
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "criteriaIcon.png",
                Command = new Command(async () => { await Navigation.PushAsync(new CriteriaPage()); })
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "plusIcon.png",
                Command = new Command(async () => { await Navigation.PushAsync(new AddingProcessPage()); }),

            });
            /*
            CurrentPage.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "refreshIcon.png",
                Command = new Command(() => { StopLoading(); })
            });
            */
        }
    }
}
