using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SpotFinder.Views
{
    public class ListPage2 : ContentPage
    {
        public ListPage2()
        {
            Title = "List";
            var listViewModel = ServiceLocator.Current.GetInstance<ListViewModel>();

            var listView = new ListView
            {
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    var viewCell = new ViewCell();
                    var image = new Image
                    {
                        WidthRequest = 120,
                        HeightRequest = 120,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center
                    };
                    image.SetBinding(Image.SourceProperty, "MainPhoto");

                    var label1 = new Label
                    {
                        TextColor = (Color)Application.Current.Resources["MainAccentColor"],
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        FontAttributes = FontAttributes.Bold
                    };
                    label1.SetBinding(Label.TextProperty, "Name");

                    var label2 = new Label
                    {
                        TextColor = (Color)Application.Current.Resources["MainAccentColor"],
                        VerticalOptions = LayoutOptions.StartAndExpand
                    };
                    label2.SetBinding(Label.TextProperty, "Type");

                    var cellLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            image,
                            new StackLayout
                            {
                                Margin = new Thickness(0,10,0,10),
                                Children =
                                {
                                    label1, label2
                                }
                            }
                        }
                    };

                    viewCell.View = cellLayout;
                    return viewCell;
                }),
                ItemsSource = listViewModel.PlaceList,
                BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"]
            };
            Content = listView;
            BindingContext = listViewModel;
            listView.ItemSelected += (s, e) =>
            {
                var place = e.SelectedItem as Place;
                if(place != null)
                {
                    listView.SelectedItem = null;
                    Navigation.PushAsync(new PlaceDetailsPage(place));
                }
            };
        }
    }
}