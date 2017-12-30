﻿using Redux;
using SimpleInjector;
using SpotFinder.Core.Enums;
using SpotFinder.DataServices;
using SpotFinder.Helpers;
using SpotFinder.Models.Core;
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
using System.Net.Http;
using System.Reactive;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

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

            simpleInjector.Register<IReducer<IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>>>, PermissionsReducer>();
            simpleInjector.Register<IReducer<Settings>, SettingsReducer>();
            simpleInjector.Register<IReducer<Stack<PageName>>, NavigationReducer>();
            simpleInjector.Register<IReducer<PlacesData>, PlaceDataReducer>();
            simpleInjector.Register<IReducer<DeviceData>, DeviceDataReducer>();

            simpleInjector.Register<IReducer<AsyncOperationState<Place, int>>, CurrentPlaceReducer>();
            simpleInjector.Register<IReducer<AsyncOperationState<IList<Place>, Criteria>>, PlacesListReducer>();
            simpleInjector.Register<IReducer<AsyncOperationState<Report, Unit>>, ReportReducer>();
        
            simpleInjector.Register(typeof(IStore<ApplicationState>), () =>
            {
                return new Store<ApplicationState>(
                    Resolve<IReducer<ApplicationState>>().Reduce,
                    Resolve<ApplicationState>()
                );
            }, Lifestyle.Singleton);

            //Helpers
            var settingsHelper = new SettingsHelper();
            simpleInjector.Register<ISettingsHelper>(() => settingsHelper);

            //Initial ApplicationState
            simpleInjector.Register(() => PrepareInitialApplicationState(settingsHelper));

            //NavigationService
            simpleInjector.Register<INavigationService, NavigationService>(Lifestyle.Singleton);

            //Data/Test services
            simpleInjector.Register<IPlaceService, PlaceService>();
            simpleInjector.Register<IPlaceRepository, PlaceRepository>();

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
            simpleInjector.Register<ISQLite>(() => DependencyService.Get<ISQLite>());

            //Other:
            var httpClient = new HttpClient();
            simpleInjector.Register(() => httpClient, Lifestyle.Singleton);
            simpleInjector.Register(() => simpleInjector.GetInstance<SettingsHelper>().ReadSettings());
            
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

        private ApplicationState PrepareInitialApplicationState(ISettingsHelper settingsHelper)
        {
            var currentPlaceState = new AsyncOperationState<Place, int>(
                Status.Empty, string.Empty, null, 0
            );
            var placesListState = new AsyncOperationState<IList<Place>, Criteria>(
                Status.Empty, string.Empty, new List<Place>(), null
            );
            var reportState = new AsyncOperationState<Report, Unit>(
                Status.Empty, string.Empty, null, Unit.Default
            );
            var placesData = new PlacesData(currentPlaceState, placesListState, reportState);

            var location = new Location(0, 0);
            var locationState = new LocationState(Status.Empty, null, location);

            var deviceData = new DeviceData(locationState);

            var permissionDictionary = new Dictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>>();
            permissionDictionary.Add(PermissionName.Camera,
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, string.Empty, PermissionStatus.Unknown, Unit.Default)
            );
            permissionDictionary.Add(PermissionName.Location,
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, string.Empty, PermissionStatus.Unknown, Unit.Default)
            );
            permissionDictionary.Add(PermissionName.Storage,
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, string.Empty, PermissionStatus.Unknown, Unit.Default)
            );
            var permissionsDictionary = permissionDictionary.ToImmutableDictionary();

            return new ApplicationState(
                permissionsDictionary, 
                new Stack<PageName>(), 
                settingsHelper.ReadSettings(), 
                placesData, 
                deviceData
            );
        }
    }
}
