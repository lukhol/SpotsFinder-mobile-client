using SpotFinder.Config;
using SpotFinder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SpotFinder.Services
{
    public class NavigationService : INavigationService
    {
        private Dictionary<Enum, Tuple<Type, Type>> PagesDictionary => new Dictionary<Enum, Tuple<Type, Type>>();

        private Page MainPage => Application.Current.MainPage;

        public void Configure(PageName pageName, Type pageType, Type viewModelType)
        {
            PagesDictionary[pageName] = new Tuple<Type, Type>(pageType, viewModelType);
        }

        public void GoBack()
        {
            MainPage.Navigation.PopAsync();
        }

        public void PopToRoot()
        {
            MainPage.Navigation.PopToRootAsync();
        }

        public void NavigateTo(PageName pageName, object parameter = null)
        {
            Tuple<Type, Type> pageAndViewModelTypesTuple;
            if (PagesDictionary.TryGetValue(pageName, out pageAndViewModelTypesTuple))
            {
                var displayPage = DIContainer.Instance.Resolve(pageAndViewModelTypesTuple.Item1) as Page;
                displayPage.BindingContext = DIContainer.Instance.Resolve(pageAndViewModelTypesTuple.Item2);
                displayPage.SetNavigationArgs(parameter);

                MainPage.Navigation.PushAsync(displayPage);
            }
            else
            {
                throw new ArgumentException($"No such page: {pageName}.", nameof(pageName));
            }
        }

        public void Navigate()
        {

        }
    }

    public static class NavigationExtensions
    {
        private static ConditionalWeakTable<Page, object> arguments = new ConditionalWeakTable<Page, object>();

        public static object GetNavigationArgs(this Page page)
        {
            object argument = null;
            arguments.TryGetValue(page, out argument);

            return argument;
        }

        public static void SetNavigationArgs(this Page page, object args)
            => arguments.Add(page, args);
    }
}

