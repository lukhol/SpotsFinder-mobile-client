using SpotFinder.Config;
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
            CreateToolbarItems();
            var listViewModel = DIContainer.Instance.Resolve<ListViewModel>();
            BindingContext = listViewModel;
        }

        public void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listViewModel = BindingContext as ListViewModel;
            listViewModel.OnListViewItemSelectedCommand.Execute(ListViewXaml.SelectedItem);
            ListViewXaml.SelectedItem = null;
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
    }
}