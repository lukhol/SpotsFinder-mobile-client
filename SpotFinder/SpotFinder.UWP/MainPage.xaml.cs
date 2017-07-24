using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SpotFinder.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            Xamarin.FormsMaps.Init("INSERT_AUTHENTICATION_TOKEN_HERE");
            this.InitializeComponent();
            LoadApplication(new SpotFinder.App());
        }
    }
}
