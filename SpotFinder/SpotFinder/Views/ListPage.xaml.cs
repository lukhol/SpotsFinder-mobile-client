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
	public partial class ListPage : ContentPage
	{
		public ListPage ()
		{
			InitializeComponent();
            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var listViewModel = (ListViewModel)serviceLocator.GetService(typeof(ListViewModel));
            BindingContext = listViewModel;
        }
	}
}