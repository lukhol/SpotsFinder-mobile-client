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
            var criteriaViewModelTwo = ServiceLocator.Current.GetInstance<CriteriaViewModel>();
            criteriaViewModelTwo.InjectPage(this);
            BindingContext = criteriaViewModelTwo;
        }
    }
}