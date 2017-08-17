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

        public UnityConfig(INavigation navigation)
        {
            unityContainer = new UnityContainer();

            //ViewModels:
            unityContainer.RegisterType<AddViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(navigation)
            );
            unityContainer.RegisterType<CriteriaViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter(navigation),
                    new ResolvedParameter<IPlaceRepository>()
                )
            );
            unityContainer.RegisterType<AddingProcessViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter(navigation),
                    new ResolvedParameter<IPlaceRepository>(),
                    new ResolvedParameter<ILocalPlaceRepository>()
                )
            );
            unityContainer.RegisterType<ListViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter(navigation), 
                    new ResolvedParameter<IPlaceRepository>()
                )
            );
            unityContainer.RegisterType<MapPageViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter(navigation),
                    new ResolvedParameter<IPlaceRepository>(),
                    new ResolvedParameter<ILocalPlaceRepository>()
                )
            );
            unityContainer.RegisterType<LocateOnMapViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter(navigation),
                    new ResolvedParameter<IPlaceRepository>()
                )
            );
            //Services:
            unityContainer.RegisterType<IPlaceRepository, PlaceRepository>(
                new InjectionConstructor(
                    new ResolvedParameter<ILocalPlaceRepository>()
                )
            );
            unityContainer.RegisterType<ILocalPlaceRepository, LocalPlaceRepository>();

            //ReportManager
            unityContainer.RegisterType<ReportManager>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<IPlaceRepository>()
                )
            );

            //Config:
            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);

            unityServiceLocator.GetInstance<MapPageViewModel>();
            unityServiceLocator.GetInstance<ListViewModel>();
            //unityServiceLocator.GetInstance<CriteriaViewModel>();
        }
    }
}
