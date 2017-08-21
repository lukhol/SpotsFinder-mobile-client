using Microsoft.Practices.ServiceLocation;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder.Views
{
	public class CriteriaPage : ContentPage
	{
		public CriteriaPage()
		{
            var criteriaViewModelTwo = ServiceLocator.Current.GetInstance<CriteriaViewModel>();
            criteriaViewModelTwo.InjectPage(this);
            BindingContext = criteriaViewModelTwo;
        }
    }
}