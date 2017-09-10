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
    public class UnityResolver
    {
        private readonly IUnityContainer unityContainer;

        private static readonly UnityResolver instance = new UnityResolver();
        
        public static UnityResolver Instance
        {
            get => instance;
        }
        protected UnityResolver()
        {
            unityContainer = new UnityContainer();

            //NavigationService
            unityContainer.RegisterType<NavigationService>(new ContainerControlledLifetimeManager());

            //ViewModels:
            unityContainer.RegisterType<ViewModels.Xaml.PlaceDetailsViewModel>();
            unityContainer.RegisterType<ViewModels.Root.MenuMasterDetailPageViewModel>();

            //Services:
            unityContainer.RegisterType<IPlaceService, PlaceService>();
            unityContainer.RegisterType<ILocalPlaceRepository, LocalPlaceRepository>();
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
