using Microsoft.Practices.ServiceLocation;
using SpotFinder.ViewModels;
using SpotFinder.ViewModels.Root;
using SpotFinder.Views;
using SpotFinder.Views.Root;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinForms;

namespace SpotFinder.Services
{
    public class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> mappings;

        protected Application CurrentApplication
        {
            get
            {
                return Application.Current;
            }
        }

        public NavigationService()
        {
            mappings = new Dictionary<Type, Type>();
            CreatePageViewModelMappings();
        }
        
        public Task InitializeAsync()
        {
            return NavigateToAsync<RootMasterDetailViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }
        
        public async Task NavigateBackAsync()
        {
            await CurrentApplication.MainPage.Navigation.PopAsync();
        }

        public async Task NavigateToRootPage()
        {
            await CurrentApplication.MainPage.Navigation.PopToRootAsync();
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

            if(page is RootMasterDetailPage)
            {
                CurrentApplication.MainPage = new NavigationPage(page)
                {
                    BarBackgroundColor = Color.Green,
                    BarTextColor = Color.White,
                    //BackgroundColor = (Color)Application.Current.Resources["PageBackgroundColor"]
                    BackgroundColor = Color.FromRgb(0, 0, 0)
                };
            }
            else
            {
                await CurrentApplication.MainPage.Navigation.PushAsync(page);
            }

        }
        
        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if(pageType == null)
            {
                throw new Exception("pageType in CreateAndBindPage is null");
            }

            var page = Activator.CreateInstance(pageType) as Page;
            var viewModel = ViewModelLocator.Instance.Resolve(viewModelType) as BaseViewModel;
            page.BindingContext = viewModel;

            if (page is IPageWithParameters)
                ((IPageWithParameters)page).InitializeWith(parameter);

            return page;
        }
        

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException(viewModelType.ToString() + "does not exist in mappings dictionary");
            }

            return mappings[viewModelType];
        }

        private void CreatePageViewModelMappings()
        {
            mappings.Add(typeof(CriteriaPage), typeof(CriteriaViewModel));
            mappings.Add(typeof(ListPage), typeof(ListViewModel));
            mappings.Add(typeof(MapPage), typeof(MapPageViewModel));
            mappings.Add(typeof(PlaceDetailsPage), typeof(PlaceDetailsViewModel));
            mappings.Add(typeof(LocateOnMapPage), typeof(LocateOnMapViewModel));
            mappings.Add(typeof(InfoPage), typeof(InfoViewModel));
            mappings.Add(typeof(SettingsPage ), typeof(SettingsViewModel));
        }
    }
}
