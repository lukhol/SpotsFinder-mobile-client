using Microsoft.Practices.ServiceLocation;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder.Views
{
    public class ListPage : ContentPage
    {
        public ListPage()
        {
            CreateToolbarItems();
            Title = "List";
            var listViewModel = ServiceLocator.Current.GetInstance<ListViewModel>();
            listViewModel.InjectPage(this);
            BindingContext = listViewModel;
        }

        private void CreateToolbarItems()
        {
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "criteriaIcon.png",
                Command = new Command(async () => { await Navigation.PushAsync(new CriteriaPage()); })
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "plusIcon.png",
                Command = new Command(async () => { await Navigation.PushAsync(new AddingProcessPage()); }),

            });
            /*
            CurrentPage.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "refreshIcon.png",
                Command = new Command(() => { StopLoading(); })
            });
            */
        }
    }
}