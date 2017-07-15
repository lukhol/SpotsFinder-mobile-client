using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SpotFinder.ViewModels;
using Xamarin.Forms;

namespace SpotFinder
{
    public class UnityConfig
    {
        private readonly UnityContainer unityContainer;

        public MainViewModel MainViewModel
        {
            get => unityContainer.Resolve<MainViewModel>();
        }

        public UnityConfig(INavigation navigation)
        {
            unityContainer = new UnityContainer();
            unityContainer.RegisterType<MainViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(navigation)
            );
            unityContainer.RegisterType<CriteriaViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(navigation)
            );
            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);

            // install a named string that holds the connection string to use
            //container.RegisterInstance<string>("MyConnectionString", connectionString, new ContainerControlledLifetimeManager());

            // register the class that will use the connection string
            //container.RegisterType<MyNamespace.MyObjectContext, MyNamespace.MyObjectContext>(new InjectionConstructor(new ResolvedParameter<string>("MyConnectionString")));

        }
    }
}
