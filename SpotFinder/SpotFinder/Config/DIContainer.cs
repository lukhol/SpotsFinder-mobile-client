using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Plugin.DeviceInfo.Abstractions;
using Redux;
using SimpleInjector;
using SpotFinder.Core.Enums;
using SpotFinder.DataServices;
using SpotFinder.Helpers;
using SpotFinder.Models.Core;
using SpotFinder.Models.DTO;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Redux.Actions.Locations;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.Actions.PlacesList;
using SpotFinder.Redux.Actions.Users;
using SpotFinder.Redux.Actions.WrongPlaceReports;
using SpotFinder.Redux.Reducers;
using SpotFinder.Redux.Reducers.Users;
using SpotFinder.Redux.StateModels;
using SpotFinder.Repositories;
using SpotFinder.Services;
using SpotFinder.SQLite;
using SpotFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;
using System.Text;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;

namespace SpotFinder.Config
{
    public class DIContainer
    {
        private const string FacebookLoginUrl = "https://www.facebook.com/dialog/oauth?client_id=204040756811030&response_type=token&redirect_uri=https://www.facebook.com/connect/login_success.html";
        private const string FacebookAppId = "204040756811030";
        private const string GoogleLoginUrl = "https://accounts.google.com/o/oauth2/v2/auth?response_type=code&scope=openid&redirect_uri=http://www.google.pl&client_id=293230980926-0k7h1g248tto4a5t98p13a5gkbvt3436.apps.googleusercontent.com";
        private const string GoogleAppId = "293230980926-0k7h1g248tto4a5t98p13a5gkbvt3436.apps.googleusercontent.com";

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
            simpleInjector.Register<IReducer<Stack<PageName>>, NavigationReducer>();

            //Place data - reporting places, displaying single place, displaying list of places:
            simpleInjector.Register<IReducer<PlacesData>, PlaceDataReducer>();
            simpleInjector.Register<IReducer<AsyncOperationState<WrongPlaceReport, Unit>>, WrongPlaceReportReducer>();
            simpleInjector.Register<IReducer<AsyncOperationState<Place, int>>, CurrentPlaceReducer>();
            simpleInjector.Register<IReducer<AsyncOperationState<IList<Place>, Criteria>>, PlacesListReducer>();
            simpleInjector.Register<IReducer<AsyncOperationState<Report, Unit>>, ReportReducer>();

            //
            simpleInjector.Register<IReducer<Settings>, SettingsReducer>();
            simpleInjector.Register<IReducer<DeviceData>, DeviceDataReducer>();
            simpleInjector.Register<IReducer<ErrorState>, ErrorReducer>();

            //User - register, login, user
            simpleInjector.Register<IReducer<UserState>, UserStateReducer>();
            simpleInjector.Register<IReducer<AsyncOperationState<User, Unit>>, RegisterReducer>();
            simpleInjector.Register<IReducer<AsyncOperationState<User, AccessProvider>>, LoginReducer>();
            simpleInjector.Register<IReducer<User>, UserReducer>();

        
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

            //Data services
            simpleInjector.Register<IPlaceService, PlaceService>();
            simpleInjector.Register<IErrorService, ErrorService>();
            simpleInjector.Register<IWrongPlaceReportService, WrongPlaceReportService>();
            simpleInjector.Register<IUserService, UserService>();
            simpleInjector.Register<IExternalUserService<SimpleFacebookUserDTO>, FacebookService>();
            simpleInjector.Register<IExternalUserService<SimpleGoogleUserDTO>, GoogleService>();

            //Repositories:
            simpleInjector.Register<IPlaceRepository, PlaceRepository>();
            var urlRepository = new URLRepository();
            simpleInjector.Register<IURLRepository>(() => urlRepository);

            //ActionsCreators:
            simpleInjector.Register<IPermissionActionCreator, PermissionActionCreator>();
            simpleInjector.Register<IDeviceLocationActionCreator, DeviceLocationActionCreator>();
            simpleInjector.Register<IGetPlacesListByCriteriaActionCreator, GetPlacesListByCriteriaActionCreator>();
            simpleInjector.Register<IGetPlaceByIdActionCreator, GetPlaceByIdActionCreator>();
            simpleInjector.Register<ISetWrongPlaceReportActionCreator, SetWrongPlaceReportActionCreator>();
            simpleInjector.Register<ILoginUserActionCreator, LoginUserActionCreator>();
            simpleInjector.Register<IExternalServiceLoginUserActionCreator, ExternalServiceLoginUserActionCreator>();
            simpleInjector.Register<IRegisterUserActionCreator, RegisterUserActionCreator>();
            
            //Providers:
            simpleInjector.Register<IPhotoProvider, PhotoProvider>();
            simpleInjector.Register<IExternalServiceUserProvider, ExternalServiceUserProvider>();

            //Bootstrapper:
            simpleInjector.Register<IBootstrapper, Bootstrapper>();

            //SQLite
            simpleInjector.Register<SQLiteConfig>(Lifestyle.Singleton);
            simpleInjector.Register<ISQLite>(() => DependencyService.Get<ISQLite>());

            //Permissions:
            simpleInjector.Register<IPermissions>(
                () => new CrossPermissionWrapper(Plugin.Permissions.CrossPermissions.Current)
            );

            //DeviceInfo:
            simpleInjector.Register<IDeviceInfo>(() => Plugin.DeviceInfo.CrossDeviceInfo.Current);

            //Other:
            var httpClient = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes(urlRepository.API_KEY);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(
                new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.ToString())
            );

            simpleInjector.Register(() => httpClient, Lifestyle.Singleton);
            simpleInjector.Register(() => CreateCameCaseJsonSerializer());
            simpleInjector.Register(() => simpleInjector.GetInstance<SettingsHelper>().ReadSettings());
            simpleInjector.Register<IErrorLogger, ErrorLogger>();

            //ViewModels
            simpleInjector.Register<LoginViewModel>(() =>
            {
                return new LoginViewModel(
                    Resolve<IStore<ApplicationState>>(),
                    GoogleLoginUrl,
                    FacebookLoginUrl,
                    Resolve<IExternalServiceLoginUserActionCreator>(),
                    Resolve<ILoginUserActionCreator>()
                );
            }, Lifestyle.Singleton);
            simpleInjector.Register<RegisterUserViewModel>(() =>
            {
                return new RegisterUserViewModel(
                    Resolve<IStore<ApplicationState>>(),
                    Resolve<IRegisterUserActionCreator>(),
                    Resolve<IPhotoProvider>()
                );
            }, Lifestyle.Singleton);

            try
            {
                simpleInjector.Verify();
            }
            catch(Exception e)
            {
                var x = 1;
            }

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
                Status.Empty, null, null, 0
            );
            var placesListState = new AsyncOperationState<IList<Place>, Criteria>(
                Status.Empty, null, new List<Place>(), null
            );
            var reportState = new AsyncOperationState<Report, Unit>(
                Status.Empty, null, null, Unit.Default
            );
            var wrongPlaceReport = new AsyncOperationState<WrongPlaceReport, Unit>(
                Status.Empty, null, null, Unit.Default
            );

            var placesData = new PlacesData(currentPlaceState, placesListState, reportState, wrongPlaceReport);

            var location = new Location(0, 0);
            var locationState = new LocationState(Status.Empty, null, location);

            var deviceData = new DeviceData(locationState);

            var permissionDictionary = new Dictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>>();
            permissionDictionary.Add(PermissionName.Camera,
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, null, PermissionStatus.Unknown, Unit.Default)
            );
            permissionDictionary.Add(PermissionName.Location,
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, null, PermissionStatus.Unknown, Unit.Default)
            );
            permissionDictionary.Add(PermissionName.Storage,
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, null, PermissionStatus.Unknown, Unit.Default)
            );
            var permissionsDictionary = permissionDictionary.ToImmutableDictionary();

            var registration = new AsyncOperationState<User, Unit>(Status.Empty, null, null, Unit.Default);
            var login = new AsyncOperationState<User, AccessProvider>(Status.Empty, null, null, AccessProvider.Unknown);
            var user = settingsHelper.ReadUser();
            var userState = new UserState(registration, login, user);

            return new ApplicationState(
                permissionsDictionary, 
                new Stack<PageName>(), 
                settingsHelper.ReadSettings(), 
                placesData, 
                deviceData,
                null,
                userState
            );
        }

        private JsonSerializer CreateCameCaseJsonSerializer()
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return jsonSerializer;
        }
    }
}
