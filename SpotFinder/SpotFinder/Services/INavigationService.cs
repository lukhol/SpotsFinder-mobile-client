using SpotFinder.Core.Enums;
using System;

namespace SpotFinder.Services
{
    public interface INavigationService
    {
        void Configure(PageName pageName, Type pageType, Type viewModelType);
        void Navigate();

        void GoBack();
        void PopToRoot();
        void NavigateTo(PageName pageName, object parameter = null);
    }
}
