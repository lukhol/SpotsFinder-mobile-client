using Microsoft.Practices.ServiceLocation;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.Views
{
    public class LocalListPage : ContentPage
    {
        public LocalListPage()
        {
            Title = "List";
            var localListViewModel = ServiceLocator.Current.GetInstance<LocalListViewModel>();
            localListViewModel.InjectPage(this);
            BindingContext = localListViewModel;
        }
    }
}
