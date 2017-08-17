using Microsoft.Practices.ServiceLocation;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder.Views
{
    public class ListPage2 : ContentPage
    {
        public ListPage2()
        {
            Title = "List";
            var listViewModel = ServiceLocator.Current.GetInstance<ListViewModel>();
            listViewModel.InjectPage(this);
            BindingContext = listViewModel;
        }
    }
}