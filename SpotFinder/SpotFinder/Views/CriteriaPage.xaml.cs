using SpotFinder.Services;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CriteriaPage : NavContentPage
	{
		public CriteriaPage()
		{
			InitializeComponent();
            BindingContext = DIContainer.Instance.Resolve<CriteriaViewModel>();
		}
	}
}