using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;


using Xamarin.Forms;

namespace SpotFinder.Views
{
	public class MapPage : ContentPage
	{
		public MapPage()
		{
            var mapPageViewModel = ServiceLocator.Current.GetInstance<MapPageViewModel>();
            mapPageViewModel.InjectPage(this, "Mapa");
            BindingContext = mapPageViewModel;
        }
	}
}