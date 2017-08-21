using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.Core;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder
{
    public class UnityConfig
    {
        private readonly UnityContainer unityContainer;

        private static readonly UnityConfig unityConfig = new UnityConfig();

        public static UnityConfig Instance
        {
            get => unityConfig;
        }

        public static void Start()
        {
            var instante = UnityConfig.Instance;
        }

        protected UnityConfig()
        {
            unityContainer = new UnityContainer();

            //ViewModels:
            unityContainer.RegisterType<CriteriaViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<AddingProcessViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ListViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<MapPageViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<LocateOnMapViewModel>(new ContainerControlledLifetimeManager() );
            //Services:
            unityContainer.RegisterType<IPlaceRepository, PlaceRepository>();
            unityContainer.RegisterType<ILocalPlaceRepository, LocalPlaceRepository>();

            //ReportManager
            unityContainer.RegisterType<ReportManager>(new ContainerControlledLifetimeManager());

            //Config:
            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);

            unityContainer.Resolve<MapPageViewModel>();
            unityContainer.Resolve<ListViewModel>();
            //unityServiceLocator.GetInstance<CriteriaViewModel>();
        }
    }
}
