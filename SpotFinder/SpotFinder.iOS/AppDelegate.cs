using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Redux;
using SpotFinder.Redux;
using SpotFinder.Redux.StateModels;
using SpotFinder.Services;
using UIKit;

namespace SpotFinder.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            LoadApplication(new App());

            AppDomain.CurrentDomain.UnhandledException += async (sender, e) =>
            {
                await SendExceptionInformationToTheServer((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
            };

            TaskScheduler.UnobservedTaskException += async (sender, e) =>
            {
                await SendExceptionInformationToTheServer(e.Exception, "TaskScheduler.UnobservedTaskException");
            };

            return base.FinishedLaunching(app, options);
        }

        private async Task SendExceptionInformationToTheServer(Exception exception, string exceptionType)
        {
            var store = Config.DIContainer.Instance.Resolve<IStore<ApplicationState>>();
            var errorLogger = Config.DIContainer.Instance.Resolve<IErrorLogger>();
            var newError = new ErrorState(exception, exceptionType);
            await errorLogger.LogErrorAsync(newError);
        }
    }
}
