using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPage : ContentPage
	{
		public AddPage ()
		{
			InitializeComponent ();
            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var mainViewModel = (MainViewModel)serviceLocator.GetService(typeof(MainViewModel));
            BindingContext = mainViewModel; 
		}
    }
}