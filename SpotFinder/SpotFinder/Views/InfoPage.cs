using SpotFinder.Core;
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
			var firstLayout = new StackLayout {
				Children = {
					new Label
                    {
                        Text = AppResources.AppDescriptionLabel,
                        TextColor = (Color)Application.Current.Resources["MainAccentColor"],
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(12)
                    }
				},
			};

            var secondLayout = new StackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = "logo.png",
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    },
                    new Label
                    {
                        Text = AppResources.AppDescriptionLabel,
                        TextColor = (Color)Application.Current.Resources["MainAccentColor"],
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Margin = new Thickness(12)
                    }
                }
            };
            Content = secondLayout;
            //Content = Utils.CreateItemOnItemLayout(secondLayout, firstLayout);
		}
	}
}