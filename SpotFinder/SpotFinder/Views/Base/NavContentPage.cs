using SpotFinder.ViewModels;
using System;
using Xamarin.Forms;

namespace SpotFinder.Views.Base
{
    public class NavContentPage : ContentPage
    {
        protected BaseViewModel baseViewModel;

        public Action CustomBackButtonAction { get; set; }

        public NavContentPage()
        {
            BackgroundColor = (Color)App.Current.Resources["PageBackgroundColor"];

            BindingContextChanged += (s, e) =>
            {
                baseViewModel = (BaseViewModel)BindingContext;
            };

            //iOS things
            if (EnableBackButtonOverride)
            {
                CustomBackButtonAction = () =>
                {
                    //tylko ios na chwilę obecną
                    //Chyba, ze odkomentuję w MainActivity obsługę
                    //baseViewModel?.GoBackCommand.Execute(null);
                    App.Current.MainPage.Navigation.PopAsync();
                };
            }
        }

        public static readonly BindableProperty EnableBackButtonOverrideProperty =
            BindableProperty.Create(
                nameof(EnableBackButtonOverride),
                typeof(bool),
                typeof(NavContentPage),
                false);

        public bool EnableBackButtonOverride
        {
            get => (bool)GetValue(EnableBackButtonOverrideProperty);
            set
            {
                SetValue(EnableBackButtonOverrideProperty, value);
            }
        }

        public virtual bool OnNavigationBackButtonPressed()
        {
            //baseViewModel?.GoBackCommand.Execute(null);
            App.Current.MainPage.Navigation.PopAsync();
            return true;
        }

        protected override bool OnBackButtonPressed()
        {
            //Tutaj logika dispatchera akcji
            //baseViewModel?.GoBackCommand.Execute(null);
            App.Current.MainPage.Navigation.PopAsync();
            return true;
        }

        ~NavContentPage()
        {
            baseViewModel.CancelSubscriptions();
        }
    }
}
