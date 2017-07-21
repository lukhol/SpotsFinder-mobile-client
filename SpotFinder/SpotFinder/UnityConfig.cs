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

        public MainViewModel MainViewModel
        {
            get => unityContainer.Resolve<MainViewModel>();
        }

        public UnityConfig(INavigation navigation)
        {
            unityContainer = new UnityContainer();

            //ViewModels:
            unityContainer.RegisterType<MainViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(navigation)
            );
            unityContainer.RegisterType<CriteriaViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(navigation)
            );
            unityContainer.RegisterType<AddingProcessViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(navigation)
            );
            unityContainer.RegisterType<ListViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter(navigation), 
                    new ResolvedParameter<IPlaceRepository>()
                )
            );

            //Services:
            unityContainer.RegisterType<IPlaceRepository, PlaceRepository>();

            //Config:
            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
        }
    }
}
