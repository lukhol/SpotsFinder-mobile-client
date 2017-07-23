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
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingProcessPage : ContentPage
    {
        private bool hasStart = false;

        public AddingProcessPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var addingProcessViewModel = (AddingProcessViewModel)serviceLocator.GetService(typeof(AddingProcessViewModel));
            BindingContext = addingProcessViewModel;
        }

        protected override async void OnAppearing()
        {
            var addingProcessViewModel = (AddingProcessViewModel)BindingContext;
            if (hasStart == false)
            {
                await addingProcessViewModel.StartAsync();
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
                var result = await this.DisplayAlert("Alert!", "Do you want cancell report?", "Yes", "No");
                if (result)
                {
                    await this.Navigation.PopToRootAsync();
                }
            });
            return true;
        }
    }
}