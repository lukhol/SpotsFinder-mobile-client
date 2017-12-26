using SpotFinder.Config;
using SpotFinder.Services;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : NavContentPage
    {
        public ListPage()
        {
            InitializeComponent();
            BindingContext = DIContainer.Instance.Resolve<ListViewModel>();
            CreateToolbarItems();
        }

        private void CreateToolbarItems()
        {
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "criteriaIcon.png",
                Command = new Command(async () => { await App.Current.MainPage.Navigation.PushAsync(new CriteriaPage()); })
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "plusIcon.png",
                Command = new Command(async () => { await App.Current.MainPage.Navigation.PushAsync(new AddingProcessPage()); }),
            });
        }

        public void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listViewModel = BindingContext as ListViewModel;
            listViewModel.OnListViewItemSelectedCommand.Execute(ListViewXaml.SelectedItem);
            ListViewXaml.SelectedItem = null;
        }
    }
}