﻿using Redux;
using SimpleInjector;
using SpotFinder.Config;
using SpotFinder.Core.Enums;
using SpotFinder.DataServices;
using SpotFinder.Helpers;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Redux.Actions.Locations;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.Actions.PlacesList;
using SpotFinder.Redux.Reducers;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using SpotFinder.Services;
using SpotFinder.SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SpotFinder.Config
{
    public class DIContainer
    {
        private readonly Container simpleInjector = new Container();

        private static readonly DIContainer instance = new DIContainer();
        
        public static DIContainer Instance
        {
            get => instance;
        }
        protected DIContainer()
        {
            //Reducers
            simpleInjector.Register<IReducer<ApplicationState>, ApplicationReducer>();

            simpleInjector.Register<IReducer<IImmutableDictionary<PermissionName, Permission>>, PermissionsReducer>();
            simpleInjector.Register<IReducer<Settings>, SettingsReducer>();
            simpleInjector.Register<IReducer<Stack<PageName>>, NavigationReducer>();
            simpleInjector.Register<IReducer<PlacesData>, PlaceDataReducer>();
            simpleInjector.Register<IReducer<DeviceData>, DeviceDataReducer>();

            simpleInjector.Register(typeof(IStore<ApplicationState>), () =>
            {
                return new Store<ApplicationState>(
                    Resolve<IReducer<ApplicationState>>().Reduce,
                    Resolve<ApplicationState>()
                );
            }, Lifestyle.Singleton);

            //State:
            simpleInjector.Register<ApplicationState>(Lifestyle.Singleton);

            //NavigationService
            simpleInjector.Register<INavigationService, NavigationService>(Lifestyle.Singleton);

            //Data/Test services
            simpleInjector.Register<IPlaceService, PlaceService>();
            simpleInjector.Register<ILocalPlaceRepository, LocalPlaceRepository>();

            //Helpers
            simpleInjector.Register<ISettingsHelper, SettingsHelper>();

            //ActionsCreators:
            simpleInjector.Register<IPermissionActionCreator, PermissionActionCreator>();
            simpleInjector.Register<IDeviceLocationActionCreator, DeviceLocationActionCreator>();
            simpleInjector.Register<IDownloadPlacesListByCriteriaActionCreator, DownloadPlacesListByCriteriaActionCreator>();
            simpleInjector.Register<IDownloadPlaceByIdActionCreator, DownloadPlaceByIdActionCreator>();
            
            //Providers:
            simpleInjector.Register<IPhotoProvider, PhotoProvider>();

            //Bootstrapper:
            simpleInjector.Register<IBootstrapper, Bootstrapper>();

            //SQLite
            simpleInjector.Register<SQLiteConfig>(Lifestyle.Singleton);

            simpleInjector.Verify();

            simpleInjector.GetInstance(typeof(SQLiteConfig));
        }

        public T Resolve<T>()
        {
            return (T)simpleInjector.GetInstance(typeof(T));
        }

        public object Resolve(Type type)
        {
            return simpleInjector.GetInstance(type);
        }
    }
}
