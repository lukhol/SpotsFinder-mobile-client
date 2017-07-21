using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
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
            var vm = (AddingProcessViewModel)BindingContext;
            await vm.StartAsync();
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
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