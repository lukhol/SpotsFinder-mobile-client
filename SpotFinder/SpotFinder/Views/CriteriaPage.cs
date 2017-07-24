using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SpotFinder.Views
{
	public class CriteriaPage : ContentPage
	{
		public CriteriaPage()
		{
            var serviceLocator = (UnityServiceLocator)ServiceLocator.Current;
            var criteriaViewModelTwo = (CriteriaViewModel)serviceLocator.GetService(typeof(CriteriaViewModel));
            criteriaViewModelTwo.InjectPage(this);
            BindingContext = criteriaViewModelTwo;
        }
    }
}