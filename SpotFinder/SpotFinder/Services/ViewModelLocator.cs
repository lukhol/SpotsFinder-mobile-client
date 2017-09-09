using Microsoft.Practices.Unity;
using SpotFinder.Core;
using SpotFinder.DataServices;
using SpotFinder.Repositories;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Services
{
    public class ViewModelLocator
    {
        private readonly IUnityContainer unityContainer;
        /*
        private static readonly ViewModelLocator instance = new ViewModelLocator();
        
        public static ViewModelLocator Instance
        {
            get => instance;
        }
            */
        protected ViewModelLocator()
        {
            unityContainer = new UnityContainer();

            //NavigationService
            unityContainer.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());

            //ViewModels:
            unityContainer.RegisterType<CriteriaViewModel>();
            unityContainer.RegisterType<AddingProcessViewModel>();
            unityContainer.RegisterType<ListViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<MapPageViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<LocateOnMapViewModel>();
            //Services:
            unityContainer.RegisterType<IPlaceService, PlaceService>();
            unityContainer.RegisterType<ILocalPlaceRepository, LocalPlaceRepository>();

            //ReportManager
            unityContainer.RegisterType<ReportManager>(new ContainerControlledLifetimeManager());
        }

        public T Resolve<T>()
        {
            return unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return unityContainer.Resolve(type);
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
