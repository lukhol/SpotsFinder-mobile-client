using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.Permissions;
using Redux;
using SpotFinder.Redux;
using SpotFinder.Redux.StateModels;
using SpotFinder.Services;
using System;
using System.Threading.Tasks;

namespace SpotFinder.Droid
{
    [Activity(Label = "Spots Finder", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true ,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

            LoadApplication(new App());
        }

        private async void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            await SendExceptionInformationToTheServer(e.Exception, "AndroidCurrentDomain");
        }

        private async void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            await SendExceptionInformationToTheServer(e.Exception, "UnobservedTaskException");
        }

        private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            await SendExceptionInformationToTheServer((Exception)e.ExceptionObject, "UnhandledExceptionRaiser");
        }

        private async Task SendExceptionInformationToTheServer(Exception exception, string exceptionType)
        {
            var store = Config.DIContainer.Instance.Resolve<IStore<ApplicationState>>();
            var errorLogger = Config.DIContainer.Instance.Resolve<IErrorLogger>();
            var newError = new ErrorState(exception, exceptionType);
            await errorLogger.LogErrorAsync(newError);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

