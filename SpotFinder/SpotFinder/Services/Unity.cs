﻿using Microsoft.Practices.Unity;
using SpotFinder.Redux.StateModels;
using SpotFinder.DataServices;
using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Repositories;
using SpotFinder.Redux.Reducers;
using System;
using System.Collections.Generic;

namespace SpotFinder.Services
{
    public class Unity
    {
        private readonly IUnityContainer unityContainer;

        private static readonly Unity instance = new Unity();
        
        public static Unity Instance
        {
            get => instance;
        }
        protected Unity()
        {
            unityContainer = new UnityContainer();

            //Reducers
            unityContainer.RegisterType<IReducer<ApplicationState>, ApplicationReducer>();

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

            //Helpers
            unityContainer.RegisterType<IPermissionHelper, PermissionHelper>();
            unityContainer.RegisterType<ISettingsHelper, SettingsHelper>();
            unityContainer.RegisterType<IDeviceLocationHelper, DeviceLocationHelper>();

            //ViewModels:
            unityContainer.RegisterType<ViewModels.Xaml.PlaceDetailsViewModel>();
            unityContainer.RegisterType<ViewModels.Root.MenuMasterDetailPageViewModel>();

            //Pages:
            unityContainer.RegisterType<Views.Xaml.PlaceDetailsPage>();
            unityContainer.RegisterType<Views.Root.Xaml.MenuMasterDetailPage>();
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