using SpotFinder.Resx;
using SpotFinder.Views;
using SpotFinder.Views.Root;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SpotFinder.ViewModels.Root
{
    public class MenuMasterDetailPageViewModel : BaseViewModel
    {
        private List<MasterPageItem> masterPageItems;

        public List<MasterPageItem> MasterPageItems
        {
            get => masterPageItems;
            set
            {
                masterPageItems = value;
                OnPropertyChanged();
            }
        }

        public MenuMasterDetailPageViewModel()
        {
            masterPageItems = new List<MasterPageItem>
            {
                new MasterPageItem
                {
                    Title = AppResources.SearchPageTitle,
                    TargetType = typeof(CriteriaPage),
                    IconSource = "criteriaIcon.png"
                },
                new MasterPageItem
                {
                    Title = AppResources.ListPageTitle,
                    TargetType = typeof(ListPage),
                    IconSource = "listIcon.png"
                },
                new MasterPageItem
                {
                    Title = AppResources.MapPageTitle,
                    TargetType = typeof(MapPage),
                    IconSource = "mapIcon.png"
                },
                new MasterPageItem
                {
                    Title = AppResources.AddPlacePageTitle,
                    TargetType = typeof(AddingProcessPage),
                    IconSource = "plusIcon.png"
                },
                new MasterPageItem
                {
                    Title = AppResources.SettingsTitle,
                    TargetType = typeof(SettingsPage),
                    IconSource = "settingsIcon.png"
                }
            };
        }
    }
}
