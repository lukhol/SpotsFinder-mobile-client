using System.Linq;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using SpotFinder.Views.Root;
using SpotFinder.Droid.Renderers;
using Android.Support.V7.Widget;
using Xamarin.Forms.Platform.Android;
using SpotFinder.Views.Base;
using Android.Support.V4.Widget;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]
namespace SpotFinder.Droid.Renderers
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        public CustomNavigationPageRenderer(Context context)
        {

        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            Element page = Element.Parent;
            MasterDetailPage masterDetailPage = null;
            while (page != null)
            {
                if (page is App)
                {
                    var app = page as App;

                    if (app == null)
                        return;

                    var navPage = app.MainPage as NavigationPage;

                    if (navPage == null)
                        return;

                    masterDetailPage = navPage.Navigation.NavigationStack.FirstOrDefault() as MasterDetailPage;
                    break;
                }

                page = page.Parent;
            }

            if (masterDetailPage == null)
            {
                return;
            }

            var renderer = Platform.GetRenderer(masterDetailPage) as MasterDetailPageRenderer;
            if (renderer == null)
            {
                return;
            }

            var drawerLayout = (DrawerLayout)renderer;
            //V4 nie działało
            Android.Support.V7.Widget.Toolbar toolbar = null;

            for (int i = 0; i < ChildCount; i++)
            {
                var child = GetChildAt(i);
                toolbar = child as Android.Support.V7.Widget.Toolbar;
                if (toolbar != null)
                {
                    break;
                }
            }

            toolbar?.SetNavigationOnClickListener(new MenuClickListener(Element, drawerLayout));
        }

        private class MenuClickListener : Java.Lang.Object, IOnClickListener
        {
            readonly NavigationPage navigationPage;
            private DrawerLayout layout;

            public MenuClickListener(NavigationPage navigationPage, DrawerLayout layout)
            {
                this.navigationPage = navigationPage;
                this.layout = layout;
            }

            public void OnClick(Android.Views.View v)
            {
                if (navigationPage.Navigation.NavigationStack.Count <= 1)
                {
                    layout.OpenDrawer((int)GravityFlags.Left);
                }
                //Dla hamburgera page jest nullem i rzucało exception bez sprawdzenia czy nie jest nullem
                //Tutaj znajduje się obsługa przycisku strony navigacji.
                var naviContentPAge = navigationPage.CurrentPage as NavContentPage;
                if(naviContentPAge != null)
                {
                    naviContentPAge.OnNavigationBackButtonPressed();
                    return;
                }

                var masterDetailPage = navigationPage.CurrentPage as MasterDetailPage;
                if(masterDetailPage != null)
                {
                    if (masterDetailPage.IsPresented)
                        masterDetailPage.IsPresented = false;
                    else
                        masterDetailPage.Title = "Spots Finder";

                    return;
                }

                var contentPage = navigationPage.CurrentPage as ContentPage;
                if(contentPage != null)
                {
                    App.Current.MainPage.Navigation.PopAsync();
                }
            }
        }
    }
}