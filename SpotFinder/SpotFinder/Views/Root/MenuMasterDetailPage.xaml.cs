﻿using SpotFinder.Config;
using SpotFinder.ViewModels.Root;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Root
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuMasterDetailPage : ContentPage
    {
        public ListView ListView { get; set; }

        public MenuMasterDetailPage()
        {
            InitializeComponent();
            BindingContext = DIContainer.Instance.Resolve<MenuMasterDetailPageViewModel>();
            ListView = ListViewXaml;
        }
    }
}