using System;
using Xamarin.Forms;

namespace SpotFinder.ViewModels.Root
{
    public class MenuMasterDetailPageViewModel
    {
        private INavigation Navigation { get; }
        private ContentPage CurrentPage { get; set; }

        public MenuMasterDetailPageViewModel(INavigation navigation)
        {
            Navigation = navigation ?? throw new ArgumentNullException("navigation is null in LocateOnMapViewModel");
        }

        public void InjectPage(ContentPage page)
        {
            CurrentPage = page;
            CurrentPage.Content = CreateMainLayout();
        }

        private StackLayout CreateMainLayout()
        {
            return null;
        }
    }
}
