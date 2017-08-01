using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.Threading.Tasks;
using SpotFinder.Resx;

namespace SpotFinder.Views
{
	public class AddingProcessPage : ContentPage
	{
        private bool hasStart = false;

        public AddingProcessPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            var addingProcessViewModel = ServiceLocator.Current.GetInstance<AddingProcessViewModel>();
            BindingContext = addingProcessViewModel;
        }

        protected override async void OnAppearing()
        {
            var addingProcessViewModel = (AddingProcessViewModel)BindingContext;
            if (hasStart == false)
            {
                addingProcessViewModel.InjectPageAsync(this, "AddingProcessPage");
                hasStart = true;
            }
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            //Only for acquiring location animation
            if (!((AddingProcessViewModel)BindingContext).CanGoBack)
            {
                return true;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert(AppResources.AlertTitle, AppResources.CancelReportAlert, AppResources.AlertYes, AppResources.AlertNo);
                if (result)
                {
                    await this.Navigation.PopToRootAsync();
                }
            });
            return true;
        }
    }
}