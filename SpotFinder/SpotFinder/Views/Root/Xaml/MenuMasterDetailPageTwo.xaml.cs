using SpotFinder.ViewModels.Root;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Views.Root
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuMasterDetailPageTwo : ContentPage
    {
        public ListView ListView { get; set; }

        public MenuMasterDetailPageTwo()
        {
            InitializeComponent();
            BindingContext = new MenuMasterDetailPageViewModel();
            ListView = ListViewXaml;
        }
    }
}