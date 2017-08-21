using SpotFinder.Resx;
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

            var logoImage = new Image
            {
                Source = "logo.png",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            if(Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.WinPhone)
            {
                //logoImage.WidthRequest = 120;
                //logoImage.HeightRequest = 120;
                logoImage.Aspect = Aspect.AspectFill;
            }

            var secondLayout = new StackLayout
            {
                Children =
                {
                    logoImage,
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