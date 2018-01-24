using SpotFinder.Config;
using SpotFinder.Redux.Actions;
using SpotFinder.ViewModels;
using SpotFinder.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : NavContentPage
    {
        private MapViewModel mapViewModel;

        public MapPage()
        {
            mapViewModel = DIContainer.Instance.Resolve<MapViewModel>();
            BindingContext = mapViewModel;
            InitializeComponent();
            CreateToolbarItems();
        }

        private void CreateToolbarItems()
        {
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "criteriaIcon.png",
                Command = new Command(async () => 
                {
                    await Navigation.PushAsync(new CriteriaPage());
                })
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "plusIcon.png",
                Command = new Command(async () => 
                {
                    mapViewModel.AppStore.Dispatch(new SetNewReportAction());
                    await Navigation.PushAsync(new AddingProcessPage());
                }),
            });
        }
    }
}