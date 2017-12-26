using SpotFinder.Config;
using SpotFinder.Services;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : NavContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            BindingContext = DIContainer.Instance.Resolve<MapViewModel>();
            CreateToolbarItems(); 
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
        }
    }
}