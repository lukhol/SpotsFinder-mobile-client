using SpotFinder.Resx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SpotFinder.Views
{
	public class InfoPage : ContentPage
	{
		public InfoPage ()
		{
            BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"];
            Title = "Info";
			Content = new StackLayout {
				Children = {
					new Label
                    {
                        Text = AppResources.AppDescriptionLabel,
                        TextColor = Color.White,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(12)
                    }
				}
			};
		}
	}
}