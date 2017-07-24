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
            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var addViewModel = (AddViewModel)serviceLocator.GetService(typeof(AddViewModel));
            addViewModel.InjectPage(this, "Add");
            BindingContext = addViewModel;
        }
	}
}