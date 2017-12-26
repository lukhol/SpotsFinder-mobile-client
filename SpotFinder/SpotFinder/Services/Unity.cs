using SpotFinder.Core.Enums;
using SpotFinder.DataServices;
using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Redux.ActionsCreators;
using SpotFinder.Redux.Reducers;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using SpotFinder.SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Unity;
using Unity.Lifetime;
using Unity.Resolution;

namespace SpotFinder.Services
{
    public class DIContainer
    {
        private readonly IUnityContainer unityContainer;

        private static readonly DIContainer instance = new DIContainer();
        
        public static DIContainer Instance
        {
            get => instance;
        }
        protected DIContainer()
        {
            unityContainer = new UnityContainer();

            //Reducers
            unityContainer.RegisterType<IReducer<ApplicationState>, ApplicationReducer>();

            unityContainer.RegisterType<IReducer<IImmutableDictionary<PermissionName, Permission>>, PermissionsReducer>();
            unityContainer.RegisterType<IReducer<Settings>, SettingsReducer>();
            unityContainer.RegisterType<IReducer<Stack<PageName>>, NavigationReducer>();
            unityContainer.RegisterType<IReducer<PlacesData>, PlaceDataReducer>();
            unityContainer.RegisterType<IReducer<DeviceData>, DeviceDataReducer>();

            //State:
            unityContainer.RegisterType<ApplicationState>();

            //NavigationService
            unityContainer.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());

            //Data/Test services
            unityContainer.RegisterType<IPlaceService, PlaceService>();
            unityContainer.RegisterType<ILocalPlaceRepository, LocalPlaceRepository>();
            RegisterSingleton<IPlaceManager, PlaceManager>();

            //Helpers
            unityContainer.RegisterType<IPermissionHelper, PermissionHelper>();
            unityContainer.RegisterType<ISettingsHelper, SettingsHelper>();

            //ActionsCreators:
            unityContainer.RegisterType<IPermissionActionCreator, PermissionActionCreator>();
            
            //Providers:
            RegisterSingleton<IDeviceLocationProvider, DeviceLocationProvider>();
            unityContainer.RegisterType<IPhotoProvider, PhotoProvider>();

            //SQLite
            unityContainer.RegisterType<SQLiteConfig>(new ContainerControlledLifetimeManager());
            unityContainer.Resolve<SQLiteConfig>();
        }

        public T Resolve<T>()
        {
            return unityContainer.Resolve<T>();
        }

        public T Resolve<T>(string param1Name, object param1)
        {
            return unityContainer.Resolve<T>(new ParameterOverride(param1Name, param1));
        }

        public T Resolve<T>(string param1Name, object param1, string param2Name, object param2)
        {
            return unityContainer.Resolve<T>(
                        new ParameterOverride(param1Name, param1),
                        new ParameterOverride(param2Name, param2)
                   );
        }

        public object Resolve(Type type)
        {
            return unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            unityContainer.RegisterType<TInterface, T>();
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
