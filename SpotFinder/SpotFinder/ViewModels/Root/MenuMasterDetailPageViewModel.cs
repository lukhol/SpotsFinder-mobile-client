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
                    Title = "Criteria",
                    TargetType = typeof(CriteriaPage),
                    IconSource = "criteriaIcon.png"
                },
                new MasterPageItem
                {
                    Title = "List",
                    TargetType = typeof(ListPage),
                    IconSource = "listIcon.png"
                },
                new MasterPageItem
                {
                    Title = "Map",
                    TargetType = typeof(MapPage),
                    IconSource = "mapIcon.png"
                },
                new MasterPageItem
                {
                    Title = "Add spot",
                    TargetType = typeof(AddingProcessPage),
                    IconSource = "plusIcon.png"
                },
                new MasterPageItem
                {
                    Title = "Settings",
                    TargetType = typeof(SettingsPage),
                    IconSource = "settingsIcon.png"
                },
                new MasterPageItem
                {
                    Title = "Offline list",
                    TargetType = typeof(LocalListPage),
                    IconSource = "listIcon.png"
                },
                new MasterPageItem
                {
                    Title = "Test page",
                    TargetType = typeof(Views.Xaml.PlaceDetailsPage),
                    IconSource = "listIcon.png"
                }
            };
        }
    }
}
