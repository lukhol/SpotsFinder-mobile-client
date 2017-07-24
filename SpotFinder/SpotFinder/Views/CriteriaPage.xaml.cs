using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CriteriaPage : ContentPage
	{
		public CriteriaPage ()
		{
			InitializeComponent ();
            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var criteriaViewModel = (CriteriaViewModel)serviceLocator.GetService(typeof(CriteriaViewModel));
            BindingContext = criteriaViewModel;
        }
	}
}