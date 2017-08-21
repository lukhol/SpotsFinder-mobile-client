using System;
using Xamarin.Forms;

namespace SpotFinder.ViewModels.Root
{
    public class MenuMasterDetailPageViewModel
    {
        private ContentPage CurrentPage { get; set; }

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
