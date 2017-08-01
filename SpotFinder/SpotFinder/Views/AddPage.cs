using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder.Views
{
	public class AddPage : ContentPage
	{
		public AddPage ()
		{
            var addViewModel = ServiceLocator.Current.GetInstance<AddViewModel>();
            addViewModel.InjectPage(this, "Add");
            BindingContext = addViewModel;
        }
	}
}